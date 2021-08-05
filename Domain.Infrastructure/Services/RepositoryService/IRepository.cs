using Domain.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Interface
{
    public interface IRepository<T> where T:class, IAuditable
    {
        Task Add(T entity);
        Task Add(IEnumerable<T>  entities);

        void Update(T entity);
        void Update(IEnumerable<T> entities);

        void Delete(T entity);
        void Delete(IEnumerable<T> entities);

        Task<IList<T>> GetAll();
        Task<T> GetById(Guid Id);


        IQueryable<T> Where(Expression<Func<T, bool>> expression);

    }
}
