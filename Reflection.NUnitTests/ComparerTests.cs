using NUnit.Framework;

namespace Reflection.NUnitTests
{
    [TestFixture]
    public class ComparerTests
    {
        private class AClassToCompareTest
        {
            public int Id { get; set; }
        }

        private class BClassToCompareTest : AClassToCompareTest
        {
            public string Name { get; set; }
        }

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public Pet[] HomePets { get; set; }
        }

        private class Pet
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public string Breed { get; set; }
        }

        [TestCase("", "", ExpectedResult = true)]
        [TestCase("AA", "AA", ExpectedResult = true)]
        [TestCase("AAB", "AA", ExpectedResult = false)]
        [TestCase("AA", "AAB", ExpectedResult = false)]
        public bool CompareString_Tests(string A, string B)
        {
            return Comparer.Compare(A, B);
        }

        [TestCase("AAB", null, ExpectedResult = false)]
        [TestCase(null, "AA", ExpectedResult = false)]
        [TestCase(null, null, ExpectedResult = true)]
        public bool CompareNull_Tests(string A, string B)
        {
            return Comparer.Compare(A, B);
        }

        [Test]
        public void CompareDifferentTypes_Tests()
        {
            Assert.IsFalse(Comparer.Compare(new AClassToCompareTest(), new BClassToCompareTest()));
        }

        [Test]
        public void CompareClassesAndEnumerables__ReturnTrue_Tests()
        {
            var person1 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = null
            };

            Assert.IsTrue(Comparer.Compare(person1, person1));

            var person2 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = new Pet[] { new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breed" } }
            };

            Assert.IsTrue(Comparer.Compare(person2, person2));
        }

        [Test]
        public void CompareClassesAndEnumerables__ReturnFalse_Tests()
        {
            var person1 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = null
            };

            var person2 = new Person()
            {
                Name = "Person name",
                Age = 26,
                HomePets = null
            };

            Assert.IsFalse(Comparer.Compare(person1, person2));

            var person3 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = new Pet[] { new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breed" } }
            };

            var person4 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = new Pet[] { new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breed" }, new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breed" } }
            };

            Assert.IsFalse(Comparer.Compare(person3, person4));

            var person5 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = new Pet[] { new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breed" } }
            };

            var person6 = new Person()
            {
                Name = "Person name",
                Age = 25,
                HomePets = new Pet[] { new Pet() { Name = "Pet name", Age = 1, Breed = "Pet breeeed" } }
            };

            Assert.IsFalse(Comparer.Compare(person5, person6));
        }
    }
}
