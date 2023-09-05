﻿using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories
{
    public interface IRepository<TEntity, TEntityId>:IQuery<TEntity> where TEntity : Entity<TEntityId>
    {
        TEntity? Get(
            Expression<Func<TEntity, bool>> predicate, //Lambda sorgusuyla istenilen bilgiye göre entitynin gelmesini sağlıyor
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, //Join işlemi yapabilmemizi sağlıyor
            bool withDeleted = false, //DB'de silinenler de getirilsin mi?
            bool enableTracking = true, //EntityFramework'ün izlenip izlenebilmesini sağlıyor.
            CancellationToken cancellationToken = default
            );
        Paginate<TEntity> GetList(
                Expression<Func<TEntity, bool>>? predicate = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, //OrderBy yapabilmemizi sağlıyor
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                int index = 0,
                int size = 10,
                bool withDeleted = false,
                bool enableTracking = true
            );
        //Dinamik Sorgulama: Örneğin araba kiralama sitesinde filtrelenecek bir sürü veri vardır. Hangi veri doldurulduysa ona göre sorgu çalıştırılır.
        Paginate<TEntity> GetListByDynamic(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0, 
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true
            );
        //Aradığımız veri var mı yok mu?
        bool Any(
            Expression<Func<TEntity, bool>>? predicate = null,
              bool withDeleted = false,
              bool enableTracking = true
            );
        TEntity Add(TEntity entity);
        ICollection<TEntity> AddRange(ICollection<TEntity> entity);
        TEntity Update(TEntity entity);
        ICollection<TEntity> UpdateRange(ICollection<TEntity> entity);
        TEntity Delete(TEntity entity, bool permanent = false); //Permanent: kalıcı mı silinsin?
        ICollection<TEntity> DeleteRange(ICollection<TEntity> entity, bool permanent = false);
    }
}
