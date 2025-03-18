using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EmployeeManagementSystemLoginHr.repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // CRUD Operations
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddMultipleAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task RemoveMultipleAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        // Query Operations
        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);
        public IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate);


    }
}
