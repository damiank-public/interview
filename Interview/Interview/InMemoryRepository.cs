using System.Collections.Generic;
using System.Linq;

namespace Interview
{
    class InMemoryRepository<T, I> : IRepository<T, I> where T : IStoreable<I>
    {
        public void Delete(I id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(T item)
        {
            throw new System.NotImplementedException();
        }

        public T Get(I id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return Enumerable.Empty<T>();
        }
    }
}
