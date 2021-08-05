using Domain.Audit;
using System;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Interface
{
    public interface IUnitOfWork<T> : IDisposable where T:class,IAuditable
    {
        IRepository<T> Repository { get; }

        Task CommitAsync();
    }
}
