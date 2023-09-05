using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Persistence.Repositories
{
    public class IEfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>, IRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TContext : DbContext
    {
        protected readonly TContext Context;

        public IEfRepositoryBase(TContext context)
        {
            Context = context;
        }

        public TEntity Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;//bölgesel saat dilimine göre
            Context.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;//bölgesel saat dilimine göre
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.CreatedDate = DateTime.UtcNow;
            Context.AddRange(entities);
            Context.SaveChanges();
            return entities;
        }

        public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.CreatedDate = DateTime.UtcNow;
            await Context.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        public bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters(); //Global Filtreleme
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return queryable.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            //Veri var mı yok mu? kontrolü sağlar 
            //silinen datalar gözükmemesi gerekiyor. withDeleted=true olarak gönderilmesi gerekiyor tüm sorgularda tek tek yapmak yerine global filtre oluşturulur.(QueryFilter).
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters(); //Global Filtreleme
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.AnyAsync(cancellationToken);
        }

        public TEntity Delete(TEntity entity, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
        {
            //Benim için permanant olup olmamasını işaretlemesini sağlayan metot. Bu metot verinin sileneceğini mi? yoksa güncelleneceğine mi karar veriyor.
            await SetEntityAsDeletedAsync(entity, permanent);
            await Context.SaveChangesAsync();
            return entity;
        }

        public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false)
        {
            await SetEntityAsDeletedAsync(entities, permanent);
            await Context.SaveChangesAsync();
            return entities;
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            return queryable.FirstOrDefault(predicate);
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if(include!=null)
                queryable=include(queryable);
            if(withDeleted)
                queryable=queryable.IgnoreQueryFilters();
            return await queryable.FirstOrDefaultAsync(predicate,cancellationToken);
        }

        public Paginate<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);
            return await queryable.ToPaginateAsync(index, size, cancellationToken);
        }

        public Paginate<TEntity> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Paginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
          
            return await queryable.ToPaginateAsync(index, size, cancellationToken);
        }

        public IQueryable<TEntity> Query() => Context.Set<TEntity>();

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;//bölgesel saat dilimine göre
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
        public ICollection<TEntity> UpdateRange(ICollection<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.UpdatedDate = DateTime.UtcNow;//bölgesel saat dilimine göre
            Context.UpdateRange(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
        {
            //Soft delete'lerde en büyük problem 1-1 ilişkisel tablolarda kaynaklanmaktadır. Çünkü bire bir olan bir tabloda soft delete yapılmış ise 1-1 olan tabloda da soft delete yapılmalıdır.
            if (!permanent)
            {
                //Soft delete ise, one to one ilişkisi var mı diye kontrol et
                CheckHasEntityHaveOneToOneRelation(entity);
                //Sonrasında ilgili tabloyu soft delete'e çeviren metottur.
                await SetEntityAsSoftDeletedAsync(entity);
            }
            else
            {
                Context.Remove(entity);
            }
        }
        protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
        {
            //Entityframework'ün metadata sayesinde her bir foreign key için one - to - one ilişkiyi tespit eden sorgudur.
            bool hasEntityHaveOneToOneRelation = Context
                                                 .Entry(entity)
                                                 .Metadata.GetForeignKeys()
                                                 .All(x =>
                                                      x.DependentToPrincipal?.IsCollection == true //one-to-one ilişkiler de tablolar Collection (koleksiyon değillerdir.) Bu yüzden  ilk tablo koleksiyon ise
                                                      || x.PrincipalToDependent?.IsCollection == true //one-to-one ilişkiler de tablolar Collection (koleksiyon değillerdir.) Bu yüzden  ikinci tablo koleksiyon ise
                                                      || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                                                        ) == false; // one - to - one ilişki değildir.
            if (hasEntityHaveOneToOneRelation)
                throw new InvalidOperationException("Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again same foreign key.");
        }
        private async Task SetEntityAsSoftDeletedAsync(IEntityTimestamps entity)
        {
            // Eğer varlık zaten silinmişse, işlemi sonlandır.
            if (entity.DeletedDate.HasValue)
                return;

            // Varlığın DeletedDate değerini güncel tarih ve saat ile ayarla.
            entity.DeletedDate = DateTime.UtcNow;

            // Varlığın ilişkili tabloların özelliklerini al.
            var navigations = Context
                .Entry(entity)
                .Metadata.GetNavigations()
                .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
                .ToList();

            // Her bir ilişkili gezinim özelliği için işlem yap.
            foreach (INavigation? navigation in navigations)
            {
                // Eğer gezinim özelliği sahibi tarafından oluşturulmuşsa (Owned), işlemi atla.
                if (navigation.TargetEntityType.IsOwned())
                    continue;

                // Eğer gezinim özelliğinin PropertyInfo bilgisi yoksa, işlemi atla.
                if (navigation.PropertyInfo == null)
                    continue;

                // Gezinim özelliğinin değerini al.
                object? navValue = navigation.PropertyInfo.GetValue(entity);

                // Eğer gezinim özelliği bir koleksiyon ise:
                if (navigation.IsCollection)
                {
                    // Eğer koleksiyonun değeri null ise, koleksiyonu yükle.
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();

                        // Eğer yüklenen koleksiyon hala null ise, işlemi atla.
                        if (navValue == null)
                            continue;
                    }

                    // Koleksiyondaki her bir öğe için yumuşak silme işlemini çağır.
                    foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                        await SetEntityAsSoftDeletedAsync(navValueItem);
                }
                // Eğer gezinim özelliği bir referans ise:
                else
                {
                    // Eğer referansın değeri null ise, referansı yükle.
                    if (navValue == null)
                    {
                        IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                            .FirstOrDefaultAsync();

                        // Eğer yüklenen referans hala null ise, işlemi atla.
                        if (navValue == null)
                            continue;
                    }

                    // Referansı yumuşak silme işlemine gönder.
                    await SetEntityAsSoftDeletedAsync((IEntityTimestamps)navValue);
                }
            }

            // Ana varlığı güncelle.
            Context.Update(entity);
        }
        //Bir nesnenin tüm ilişkilerini yakalayan metottur
        protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
        {
            Type queryProviderType = query.Provider.GetType();
            MethodInfo createQueryMethod =
                queryProviderType
                    .GetMethods()
                    .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                    ?.MakeGenericMethod(navigationPropertyType)
                ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
            var queryProviderQuery =
                (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
            return queryProviderQuery.Where(x => !((IEntityTimestamps)x).DeletedDate.HasValue);
        }

        protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
        {
            foreach (TEntity entity in entities)
                await SetEntityAsDeletedAsync(entity, permanent);
        }


    }
}
