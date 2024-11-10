using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Repositories
{
    internal abstract class Repository<T> where T: Entity
    {
        protected readonly ApplicationDbContext dbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext
                .Set<T>()
                .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }

        public virtual void Add(T entity)
        {
            dbContext.Add(entity);
        }
    }
}
