using BS.Application.Contracts.Persistence;
using BS.Application.Features.Queries.Models;
using BS.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BSDbContext _db;
        private DbSet<T> dbSet;

        public GenericRepository(BSDbContext db)
        {
            this._db = db;
            this.dbSet = _db.Set<T>();
        }


        public async Task<CommandResponse> CreateAsync(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                await _db.SaveChangesAsync();
                return new CommandResponse { isSuccess = true };
            }
            catch(Exception ex)
            {
                return new CommandResponse { isSuccess = false, Message = ex.Message };
            }
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] {','}, 
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
    
                }
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            var query = dbSet.Where(filter).AsNoTracking();

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] {','},
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> PaginationFilter(IEnumerable<T> entities, QueryParameters queryParameters)
        {
            entities = entities
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return entities;
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

    }
}
