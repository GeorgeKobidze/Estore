using Domain.Audit;
using Domain.Infrastructure.Interface;
using System.Threading.Tasks;

namespace Domain.Infrastructure.NewFolder
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class, IAuditable
    {
        private readonly EStoreDBContext _eStoreDBContext;
        public IRepository<T> Repository { get; }

        public UnitOfWork(EStoreDBContext eStoreDBContext)
        {
            _eStoreDBContext = eStoreDBContext;
            Repository = new Repository<T>(_eStoreDBContext);
        }

        

        public async Task CommitAsync()
        {

            await _eStoreDBContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _eStoreDBContext.Dispose();
        }
    }
}
