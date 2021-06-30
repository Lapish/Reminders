using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reminders.Core.Repositories.Interfaces;

namespace Reminders.Core.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Methods

        #region Public

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            using(var context = new ReminderContext())
            {
                try
                {
                    return await context.Set<TEntity>().Take(20).ToListAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Couldn't retrieve entities: {ex.Message}");
                }
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var context = new ReminderContext())
            {
                if (entity == null)
                {
                    throw new Exception($"{nameof(AddAsync)} entity must not be null");
                }

                try
                {
                    await context.Set<TEntity>().AddAsync(entity);
                    await context.SaveChangesAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
                }
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (var context = new ReminderContext())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
                }

                try
                {
                    context.Set<TEntity>().Update(entity);
                    await context.SaveChangesAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
                }
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using(var context = new ReminderContext())
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #endregion
    }
}