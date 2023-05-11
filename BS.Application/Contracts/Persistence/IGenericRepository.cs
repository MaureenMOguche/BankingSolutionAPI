using BS.Application.Features.Queries.Models;
using BS.Application.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null,
            Expression<Func<T, bool>>? filter = null);
        Task<T> GetOneAsync(Expression<Func<T,bool>> filter, string? includeProperties = null);
        Task<CommandResponse> CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<IEnumerable<T>> PaginationFilter(IEnumerable<T> entities, QueryParameters query);
    }
}
