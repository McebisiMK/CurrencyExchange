using System.Linq.Expressions;

namespace CurrencyExchange.Application.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Update(TEntity entity);
        IQueryable<TEntity> GetAll();
        Task AddAsync(TEntity entity);
        bool Exists(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
