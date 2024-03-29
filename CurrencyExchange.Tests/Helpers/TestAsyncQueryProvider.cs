using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Text.Json;

namespace CurrencyExchange.Tests.Helpers
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _queryProvider;

        internal TestAsyncQueryProvider(IQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _queryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _queryProvider.Execute<TResult>(expression);
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var dynamicExpression = Expression.Lambda(expression).Compile().DynamicInvoke();
            return Task.FromResult(dynamicExpression as dynamic);
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>, IAsyncDisposable, IDisposable
    {
        private readonly IEnumerator<T> _enumerator;
        private Utf8JsonWriter? _jsonWriter = new(new MemoryStream());

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _jsonWriter?.Dispose();
                _jsonWriter = null;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_jsonWriter is not null)
            {
                await _jsonWriter.DisposeAsync().ConfigureAwait(false);
            }

            _jsonWriter = null;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public T Current
        {
            get { return _enumerator.Current; }
        }
    }
}
