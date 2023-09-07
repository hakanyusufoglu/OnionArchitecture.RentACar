namespace Core.Application.Pipelines.Caching
{
    public interface ICachableRequest
    {
        string CacheKey { get; }
        //Cache'i iptal edebilmek için
        bool BypassCache { get; }
        //CacheGroupKey property'sinin olmasının amacı örneğin brand tablosuna ait cachelerde özel keyler olmasından kaynaklanmaktadır. Her bir sayfalama için özel cacheleme yapan bir sistem yapmıştık. bu yüzden bu cacheleri bir grup altında toplamalıyız.
        string? CacheGroupKey { get; }
        //Cache de ne kadar duracak ve bu bilgil genelde appsettings'den okunur.
        TimeSpan? SlidingExpiration { get; } 
    }
}

