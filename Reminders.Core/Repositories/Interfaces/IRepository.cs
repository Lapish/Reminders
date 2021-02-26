using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Core.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task DeleteAsync(TEntity entity);
    }
}