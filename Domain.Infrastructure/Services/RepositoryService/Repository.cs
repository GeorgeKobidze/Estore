using Domain.Audit;
using Domain.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Infrastructure.NewFolder
{
    public class Repository<T> : IRepository<T> where T : class, IAuditable
    {
        private readonly EStoreDBContext _eStoreDBContext;
        
        public Repository(EStoreDBContext eStoreDBContext)
        {
            _eStoreDBContext = eStoreDBContext;
        }
        public async Task Add(T entity)
        {
            await _eStoreDBContext.AddAsync<T>(entity);
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _eStoreDBContext.AddRangeAsync(entities);
        }

        public  void Delete(T entity)
        {
            _eStoreDBContext.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _eStoreDBContext.RemoveRange(entities);
        }

        public async Task<IList<T>> GetAll()
        {
            return await _eStoreDBContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await _eStoreDBContext.Set<T>().AsNoTracking().SingleAsync(x => x.Uid == Id);
        }

        public void Update(T entity)
        {
            _eStoreDBContext.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            _eStoreDBContext.UpdateRange(entities);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _eStoreDBContext.Set<T>().Where<T>(expression);
        }
    }
}
