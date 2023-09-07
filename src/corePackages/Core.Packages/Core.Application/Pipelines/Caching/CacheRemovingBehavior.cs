using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Core.Application.Pipelines.Caching
{
    public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheRemoverRequest
    {
        private readonly IDistributedCache _cache;

        public CacheRemovingBehavior(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Önbelleği atlamak için istek BypassCache olarak işaretlenmişse, isteği doğrudan işle ve cevabı dön
            if (request.BypassCache) { return await next(); }

            // İsteği işle ve cevabı al
            TResponse response = await next();

            // Eğer istekte bir CacheGroupKey belirtilmişse, grup önbelleğini temizle
            if (request.CacheGroupKey != null)
            {
                // CacheGroupKey ile ilişkilendirilmiş önbellek verisini al
                byte[]? cachedGroup = await _cache.GetAsync(request.CacheGroupKey, cancellationToken);

                // Eğer önbellekte bu grup varsa
                if (cachedGroup != null)
                {
                    // Önbellekteki veriyi HashSet olarak deserialize et
                    HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup))!;

                    // Gruptaki her öğeyi önbellekten kaldır
                    foreach (string key in keysInGroup)
                    {
                        await _cache.RemoveAsync(key, cancellationToken);
                    }

                    // Grup anahtarını ve ilişkili sürekli yenileme anahtarını kaldır
                    await _cache.RemoveAsync(request.CacheGroupKey, cancellationToken);
                    await _cache.RemoveAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken);
                }
            }

            // Eğer istekte bir CacheKey belirtilmişse, bu anahtarı önbellekten kaldır
            if (request.CacheKey != null)
            {
                await _cache.RemoveAsync(request.CacheKey);
            }

            return response;
        }

    }
}
