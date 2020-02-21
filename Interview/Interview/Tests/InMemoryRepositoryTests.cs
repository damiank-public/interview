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
        public void ctor_ThrowsArgumentNullExceptionIfParameterIsNull()
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
            var items = new Dictionary<int, IStoreable<int>>
            {
                [1] = new TestStoreable { Id = 1 },
                [2] = new TestStoreable { Id = 2 },
                [3] = new TestStoreable { Id = 3 }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(items);

            var actual = inMemoryRepository.GetAll();

            Assert.That(actual, Has.Exactly(3).Items);
            Assert.That(actual, Has.One.Items.Matches<IStoreable<int>>(x => x.Id == 1));
            Assert.That(actual, Has.One.Items.Matches<IStoreable<int>>(x => x.Id == 2));
            Assert.That(actual, Has.One.Items.Matches<IStoreable<int>>(x => x.Id == 3));
        }

        [Test]
        public void Get_ReturnsMatchedItem()
        {
            var items = new Dictionary<int, IStoreable<int>>
            {
                [1] = new TestStoreable { Id = 1 },
                [2] = new TestStoreable { Id = 2 },
                [3] = new TestStoreable { Id = 3 }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(items);

            var actual = inMemoryRepository.Get(2);

            Assert.That(actual.Id, Is.EqualTo(2));

        }

        private class TestStoreable : IStoreable<int>
        {
            public int Id { get; set; }
        }
    }
}
