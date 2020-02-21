using System;
using System.Collections.Generic;

namespace Interview
{
    class InMemoryRepository<T, I> : IRepository<T, I> where T : IStoreable<I>
    {
        private readonly Dictionary<I, T> _storage;

        public InMemoryRepository(Dictionary<I, T> storage)
        {
            if (storage is null) throw new ArgumentNullException(nameof(storage));

            _storage = storage;
        }

        public void Delete(I id)
        {
            if (!_storage.ContainsKey(id)) throw new ItemNotFoundException();

            _storage.Remove(id);
        }

        public void Save(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            _storage[item.Id] = item;
        }

        public T Get(I id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), $"{nameof(id)} cannot be null");
            if (!_storage.ContainsKey(id)) throw new ItemNotFoundException();

            return _storage[id];
        }

        public IEnumerable<T> GetAll()
        {
            return _storage.Values;
        }
    }
}
