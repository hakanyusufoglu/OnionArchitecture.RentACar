using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Core.Application.Pipelines.Caching
{
    //Sen bu requestte ICachableRequest varsa çalış diyoruz
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICachableRequest
    {
        private readonly CacheSettings _cacheSettings;
        //Distributed cache böylece farklı cache ortamlarına geçebiliyoruz redis, in-memory vb 
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCache cache, IConfiguration configuration, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            //appsettings.jsondaki CacheSettings'i node'un CacheSettings nesnesine aktarıyor.
            _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.BypassCache) { return await next(); } //Sen hiç cache içerisine girme, devam et

            TResponse response;
            byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

            //cachedResponse boş değilse data cache'da var ve veri tabanına gitme diyoruz
            if (cachedResponse != null)
            {
                //Byte datayı deserialize et
                response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
            }
            else //Data cache'e eklenip hem de dbye kaydetmesi gerekiyor
            {

                response = await GetResponseAndAddToCache(request, next, cancellationToken);
            }
            return response;
        }

        private async Task<TResponse?> GetResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Önce Db'den datayı al
            TResponse response = await next();//metodu çalıştır.

            //Eğer SlidingExpiration süresi yoksa appsettingsden süreyi alsın ve o süre içinde cache de kalsın
            TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = slidingExpiration };

            //Response ByteArray'e çevrilmesi gerekiyor
            byte[] serializeData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

            //veri cache'e eklenildi
            await _cache.SetAsync(request.CacheKey, serializeData, cancellationToken);
            _logger.LogInformation($"Added to Cache -> {request.CacheKey}");

            //Her bir brand cachei için özel key kullandığımız için bu cacheleri gruplamamız gerekir
            if (request.CacheGroupKey != null)
                await AddCacheKeyToGroup(request, slidingExpiration, cancellationToken);
            return response;

        }

        private async Task AddCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
        {
            // Önbellekten cacheGroupCache'i al
            byte[]? cacheGroupCache = await _cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);
            HashSet<string> cacheKeysInGroup;

            // Eğer cacheGroupCache boş değilse, içeriğini al ve işle
            if (cacheGroupCache != null)
            {
                // JSON formatından HashSet'e dönüştür
                cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;

                // Eğer cacheKeysInGroup içinde istenen anahtar yoksa, ekleyin
                if (!cacheKeysInGroup.Contains(request.CacheKey))
                    cacheKeysInGroup.Add(request.CacheKey);
            }
            else
            {
                // cacheGroupCache boşsa yeni bir HashSet oluşturun ve istenen anahtarı ekleyin
                cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
            }

            // HashSet'i JSON formatına çevirip yeniCacheGroupCache'e kaydedin
            byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

            // Önbellekten cacheGroupCacheSlidingExpirationCache'i al
            byte[]? cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken);

            int? cacheGroupCacheSlidingExpirationValue = null;
            // Eğer cacheGroupCacheSlidingExpirationCache boş değilse, içeriği alın ve değeri alın
            if (cacheGroupCacheSlidingExpirationCache != null)
                cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));

            // Eğer cacheGroupCacheSlidingExpirationValue null ise veya slidingExpiration daha büyükse, yeni değeri ayarlayın
            if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
                cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);

            // cacheGroupCacheSlidingExpirationValue'yu JSON formatına çevirin
            byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

            // Dağıtılmış Önbellek Giriş Seçeneklerini oluşturun
            DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

            // cacheGroupKey'i kullanarak yeniCacheGroupCache'i kaydedin
            await _cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);
            _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");

            // cacheGroupKeySlidingExpiration'ı kullanarak cacheGroupCacheSlidingExpirationValue'yi kaydedin
            await _cache.SetAsync(
                key: $"{request.CacheGroupKey}SlidingExpiration",
                serializeCachedGroupSlidingExpirationData,
                cacheOptions, cancellationToken
            );
            _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}SlidingExpiration");
        }

    }
}

//Cached data

//CacheGroupKey - cacheKeys[]
