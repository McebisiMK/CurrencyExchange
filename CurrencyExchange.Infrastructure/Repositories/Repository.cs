using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurrencyExchange.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly CurrencyExchangeContext _currencyExchangeContext;

        public Repository(CurrencyExchangeContext currencyExchangeContext)
        {
            _currencyExchangeContext = currencyExchangeContext;
            _dbSet = _currencyExchangeContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Any(expression);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _currencyExchangeContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
