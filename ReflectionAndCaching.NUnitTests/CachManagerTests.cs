using System.Threading;
using NUnit.Framework;

namespace ReflectionAndCaching.NUnitTests
{
    [TestFixture]
    public class CachManagerTests
    {
        [Cach(0, 0, 3)]
        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        private Person _person;
        private string _key;

        [SetUp]
        public void Initialize()
        {
            _person = new Person() { Name = "Name", Age = 25 };
            _key = "person";
        }

        [Test]
        public void SetAndGetValueFromCach_Tests()
        {
            CachManager.SetInCach(_person, _key);
            Person person = CachManager.GetFromCach<Person>(_key);

            Assert.AreEqual(_person.Name, person.Name);
            Assert.AreEqual(_person.Age, person.Age);
        }

        [Test]
        public void SetValue_CachingTimeEnd_Tests()
        {
            CachManager.SetInCach(_person, _key);

            Thread.Sleep(5000);
            var person = CachManager.GetFromCach<Person>(_key);

            Assert.IsNull(person);
        }
    }
}
