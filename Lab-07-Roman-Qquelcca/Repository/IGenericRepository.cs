using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Lab_07_Roman_Qquelcca.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<T?> GetById(int id);
        Task<T?> GetByIdString(string id);

        
        Task<List<T>> GetByIds(
            IEnumerable<int> ids,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IEnumerable<T>> GetByStringProperty(
            string propertyName,
            string value,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}