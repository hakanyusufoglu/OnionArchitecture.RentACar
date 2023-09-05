using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories
{
    //IAsyncRepository tüm projelerimde kullanabileceğimden Core.Packages projesinde tanımmlandı
    //Çalışacağım Entity'nin id'si farklı türden olabileceğinden TEntityId generic'ini sağladık
    //where TEntity : Entity<TEntityId> gelen TEntity'nin Entity<TEntityId> olmasını bekliyorum anlamındadır.
    public interface IAsyncRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
    {
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate, //Lambda sorgusuyla istenilen bilgiye göre entitynin gelmesini sağlıyor
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, //Join işlemi yapabilmemizi sağlıyor
            bool withDeleted = false, //DB'de silinenler de getirilsin mi?
            bool enableTracking = true, //EntityFramework'ün izlenip izlenebilmesini sağlıyor.
            CancellationToken cancellationToken = default
            );
        Task<Paginate<TEntity>> GetListAsync(
                Expression<Func<TEntity, bool>>? predicate = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, //OrderBy yapabilmemizi sağlıyor
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                int index = 0,
                int size = 10,
                bool withDeleted = false,
                bool enableTracking = true,
                CancellationToken cancellationToken = default
            );
        //Dinamik Sorgulama: Örneğin araba kiralama sitesinde filtrelenecek bir sürü veri vardır. Hangi veri doldurulduysa ona göre sorgu çalıştırılır.
        Task<Paginate<TEntity>> GetListByDynamicAsync(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate=null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
            );
        //Aradığımız veri var mı yok mu?
        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
              bool withDeketed = false,
              bool enableTracking = true,
              CancellationToken cancellationToken = default
            );
        Task<TEntity> AddAsync(TEntity entity);
        Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
        Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false); //Permanent: kalıcı mı silinsin?
        Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);
    }
}
