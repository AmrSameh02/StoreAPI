using Store.Route.Core.Entities;
using Store.Route.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        //Create Repository<T> And return
        IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
