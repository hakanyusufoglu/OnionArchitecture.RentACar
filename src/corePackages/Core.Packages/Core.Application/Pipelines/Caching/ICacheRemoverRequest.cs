namespace Core.Application.Pipelines.Caching
{
    public interface ICacheRemoverRequest
    {
        string? CacheKey { get; }
        //Cache'i iptal edebilmek için
        bool BypassCache { get; }
        string? CacheGroupKey { get; }
    }
}
