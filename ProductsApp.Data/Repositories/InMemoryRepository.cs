using System.Collections.Concurrent;
using System.Linq.Expressions;
using ProductsApp.Domain.Interfaces.Repositories;
using ProductsApp.Domain.Models;

namespace ProductsApp.Data.Repositories
{
    public abstract class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ConcurrentDictionary<int, T> _store = new();
        private int _idSeed;

        public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult(_store.Values.AsEnumerable());

        public Task<T?> GetByIdAsync(int id)
        {
            _store.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public Task<T> CreateAsync(T entity)
        {
            var id = Interlocked.Increment(ref _idSeed);
            entity.Id = id;
            _store[id] = entity;
            return Task.FromResult(entity);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            if (!_store.ContainsKey(entity.Id)) return Task.FromResult(false);
            _store[entity.Id] = entity;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id) => Task.FromResult(_store.TryRemove(id, out _));

        public Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            var compiledPredicate = predicate.Compile();
            var results = _store.Values.Where(compiledPredicate).AsEnumerable();
            return Task.FromResult(results);
        }
    }
}
