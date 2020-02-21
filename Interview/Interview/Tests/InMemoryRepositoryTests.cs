using NUnit.Framework;

namespace Interview.Tests
{
    [TestFixture]
    public class InMemoryRepositoryTests
    {
        [Test]
        public void CanConstruct()
        {
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>();
        }

        [Test]
        public void GetAll_InitiallyReturnsNoItems()
        {
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>();

            var actual = inMemoryRepository.GetAll();

            Assert.That(actual, Is.Empty);
        }

    }
}
