using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> Count(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetPagedReponseAsync(int pageNumber, int pageSize,
                                                  Expression<Func<T, bool>> filter,
                                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                  string includeProperties = "");
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
                                      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                      string includeProperties = "");
        Task<T> FirstAsync(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<T> GetByIdAsync(object id);
        Task<T> AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteAllAsync(IEnumerable<T> entities);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
    }
}
