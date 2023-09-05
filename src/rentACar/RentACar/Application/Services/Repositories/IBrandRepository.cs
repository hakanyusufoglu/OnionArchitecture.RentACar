using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories
{
    //Asenkron işlemler için IAsyncRepository<T>, 
    //Senkron işlemler için IRepository<T>
    public interface IBrandRepository:IAsyncRepository<Brand,Guid>, IRepository<Brand,Guid>
    {
    }
}
