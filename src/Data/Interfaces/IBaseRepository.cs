

using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null);
    void Update(TEntity entity);
    void Delete(TEntity entity);

    Task<int> SaveChangesAsync();

    Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression);

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
