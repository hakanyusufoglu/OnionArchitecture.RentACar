namespace Core.Persistence.Repositories
{
    //Direkt olarak veri tabanı sorgusu yazabileceğimiz interface'dir
    public interface IQuery<T>
    {
        IQueryable<T> Query();
    }
}
