namespace Core.Persistence.Repositories
{
    public interface IEntityTimestamps
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; } //ilk nesne girildiğinde Update tarihi girilmesi zorunda olmadığımdan nullable yapıldı.
        DateTime? DeletedDate { get; set; } //ilk nesne girildiğinde Delete tarihi girilmesi zorunda olmadığımdan nullable yapıldı.
    }
}
