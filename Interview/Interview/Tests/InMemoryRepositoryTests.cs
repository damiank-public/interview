using NUnit.Framework;
using System.Collections.Generic;

namespace Interview.Tests
{
    [TestFixture]
    public class InMemoryRepositoryTests
    {
        [Test]
        public void CanConstruct()
        {
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>>());
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfParameterIsNull()
        {
            Assert.That(() => new InMemoryRepository<IStoreable<int>, int>(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetAll_InitiallyReturnsNoItems()
        {
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>>());

            var actual = inMemoryRepository.GetAll();

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetAll_ReturnsAllItems()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 });

            var actual = inMemoryRepository.GetAll();

            Assert.That(actual, Is.EquivalentTo(new List<TestStoreable<int>> { item1, item2, item3 }));
        }

        [Test]
        public void Get_ReturnsMatchedItem()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 });

            var actual = inMemoryRepository.Get(2);

            Assert.That(actual, Is.EqualTo(item2));
        }

        [Test]
        public void Get_ReturnsMatchedItemForReferenceTypeId()
        {
            var item1 = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(1) };
            var item2 = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(2) };
            var item3 = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(3) };
            var storage = new Dictionary<TestReferenceTypeId, IStoreable<TestReferenceTypeId>>
            {
                [new TestReferenceTypeId(1)] = item1,
                [new TestReferenceTypeId(2)] = item2,
                [new TestReferenceTypeId(3)] = item3
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<TestReferenceTypeId>, TestReferenceTypeId>(storage);

            var actual = inMemoryRepository.Get(new TestReferenceTypeId(2));

            Assert.That(actual, Is.EqualTo(item2));
        }

        [Test]
        public void Get_ThrowsItemNotFoundExceptionIfNoMatch()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 });

            Assert.That(() => inMemoryRepository.Get(4), Throws.InstanceOf<ItemNotFoundException>());
        }

        [Test]
        public void Get_ThrowsArgumentNullExceptionIfIdIsNull()
        {
            var storage = new Dictionary<int?, IStoreable<int?>>();
            var inMemoryRepository = new InMemoryRepository<IStoreable<int?>, int?>(storage);

            Assert.That(() => inMemoryRepository.Get(null), Throws.ArgumentNullException.With.Message.Contains("id cannot be null"));
        }

        [Test]
        public void Save_AddsItemToStorage()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var storage = new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(storage);

            var newItem = new TestStoreable<int> { Id = 4 };
            inMemoryRepository.Save(newItem);

            Assert.That(storage[4], Is.EqualTo(newItem));
        }

        [Test]
        public void Save_ThrowsArgumentNullExceptionIfItemIsNull()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 });

            Assert.That(() => inMemoryRepository.Save(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Save_UpdatesExistingItemIfAlreadyInStorage()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var storage = new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(storage);

            var newItem = new TestStoreable<int> { Id = 2 };
            inMemoryRepository.Save(newItem);

            Assert.That(storage[2], Is.EqualTo(newItem));
        }

        [Test]
        public void Delete_RemovesItemIfIdMatches()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var storage = new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(storage);

            inMemoryRepository.Delete(2);

            Assert.That(storage, Does.Not.ContainKey(2));
        }

        [Test]
        public void Delete_ThrowsItemNotFoundExceptionIfIdDoesNotMatch()
        {
            var item1 = new TestStoreable<int> { Id = 1 };
            var item2 = new TestStoreable<int> { Id = 2 };
            var item3 = new TestStoreable<int> { Id = 3 };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(new Dictionary<int, IStoreable<int>> { [1] = item1, [2] = item2, [3] = item3 });

            Assert.That(() => inMemoryRepository.Delete(4), Throws.InstanceOf<ItemNotFoundException>());
        }

        [Test]
        public void Delete_ThrowsArgumentNullExceptionIfIdIsNull()
        {
            var storage = new Dictionary<int?, IStoreable<int?>>();
            var inMemoryRepository = new InMemoryRepository<IStoreable<int?>, int?>(storage);

            Assert.That(() => inMemoryRepository.Delete(null), Throws.ArgumentNullException.With.Message.Contains("id cannot be null"));
        }

        private class TestStoreable<T> : IStoreable<T>
        {
            public T Id { get; set; }
        }

        private class TestReferenceTypeId
        {
            private readonly int _id;

            public TestReferenceTypeId(int id)
            {
                _id = id;
            }

            public override bool Equals(object obj)
            {
                return obj is TestReferenceTypeId key &&
                       _id == key._id;
            }

            public override int GetHashCode()
            {
                return 1969571243 + _id.GetHashCode();
            }
        }
    }
}
