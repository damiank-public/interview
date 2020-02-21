﻿using NUnit.Framework;
using System;
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
                [1] = new TestStoreable<int> { Id = 1, Value = "First test storeable" },
                [2] = new TestStoreable<int> { Id = 2, Value = "Second test storeable" },
                [3] = new TestStoreable<int> { Id = 3, Value = "Third test storeable" }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(items);

            var actual = inMemoryRepository.GetAll();

            Assert.That(actual, Has.Exactly(3).Items);
            Assert.That(actual, Has.One.Items.Matches<TestStoreable<int>>(x => x.Value == "First test storeable"));
            Assert.That(actual, Has.One.Items.Matches<TestStoreable<int>>(x => x.Value == "Second test storeable"));
            Assert.That(actual, Has.One.Items.Matches<TestStoreable<int>>(x => x.Value == "Third test storeable"));
        }

        [Test]
        public void Get_ReturnsMatchedItem()
        {
            var items = new Dictionary<int, IStoreable<int>>
            {
                [1] = new TestStoreable<int> { Id = 1, Value = "First test storeable" },
                [2] = new TestStoreable<int> { Id = 2, Value = "Second test storeable" },
                [3] = new TestStoreable<int> { Id = 3, Value = "Third test storeable" }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(items);

            var actual = inMemoryRepository.Get(2) as TestStoreable<int>;

            Assert.That(actual.Value, Is.EqualTo("Second test storeable"));
        }

        [Test]
        public void Get_ReturnsMatchedItemForReferenceTypeId()
        {
            var items = new Dictionary<TestReferenceTypeId, IStoreable<TestReferenceTypeId>>
            {
                [new TestReferenceTypeId(1)] = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(1), Value = "First test storeable" },
                [new TestReferenceTypeId(2)] = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(2), Value = "Second test storeable" },
                [new TestReferenceTypeId(3)] = new TestStoreable<TestReferenceTypeId> { Id = new TestReferenceTypeId(3), Value = "Third test storeable" }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<TestReferenceTypeId>, TestReferenceTypeId>(items);

            var actual = inMemoryRepository.Get(new TestReferenceTypeId(2)) as TestStoreable<TestReferenceTypeId>;

            Assert.That(actual.Value, Is.EqualTo("Second test storeable"));
        }

        [Test]
        public void Get_ThrowsItemNotFoundExceptionIfNoMatch()
        {
            var items = new Dictionary<int, IStoreable<int>>
            {
                [1] = new TestStoreable<int> { Id = 1, Value = "First test storeable" },
                [2] = new TestStoreable<int> { Id = 2, Value = "Second test storeable" },
                [3] = new TestStoreable<int> { Id = 3, Value = "Third test storeable" }
            };
            var inMemoryRepository = new InMemoryRepository<IStoreable<int>, int>(items);

            Assert.That(() => inMemoryRepository.Get(4), Throws.InstanceOf<ItemNotFoundException>());
        }

        [Test]
        public void Get_ThrowsArgumentNullExceptionIfIdIsNull()
        {
            var items = new Dictionary<int?, IStoreable<int?>>();
            var inMemoryRepository = new InMemoryRepository<IStoreable<int?>, int?>(items);

            Assert.That(() => inMemoryRepository.Get(null), Throws.ArgumentNullException.With.Message.Contains("id cannot be null"));
        }

        private class TestStoreable<T> : IStoreable<T>
        {
            public T Id { get; set; }
            public string Value { get; set; }
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
