using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace CurrencyExchange.Tests.Helpers
{
    public static class TestCustomExtensions
    {
        public static DbSet<TEntity> Initialize<TEntity>(this DbSet<TEntity> dbSet, IQueryable<TEntity> data) where TEntity : class
        {
            ((IAsyncEnumerable<TEntity>)dbSet).GetAsyncEnumerator().Returns(new TestAsyncEnumerator<TEntity>(data.GetEnumerator()));
            ((IQueryable<TEntity>)dbSet).Provider.Returns(new TestAsyncQueryProvider<TEntity>(data.Provider));
            ((IQueryable<TEntity>)dbSet).Expression.Returns(data.Expression);
            ((IQueryable<TEntity>)dbSet).ElementType.Returns(data.ElementType);

            return dbSet;
        }
    }
}
