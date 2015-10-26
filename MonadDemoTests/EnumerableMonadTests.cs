using System;
using System.Linq;
using MonadDemo;
using NUnit.Framework;

namespace MonadDemoTests
{
    [TestFixture]
    public class EnumerableMonadTests
    {
        [Test]
        public void Return()
        {
            var m = EnumerableMonad.Return(5);
            Assert.That(m, Is.EqualTo(new[] {5}));
        }

        [Test]
        public void Map()
        {
            var m1 = Enumerable.Range(1, 3);
            var m2 = m1.Map(Convert.ToString);
            Assert.That(m2, Is.EqualTo(new[] {"1", "2", "3"}));
        }

        [Test]
        public void FlatMap()
        {
            var m1 = Enumerable.Range(1, 3);
            var m2 = m1.FlatMap(n => Enumerable.Repeat(Convert.ToString(n), n));
            Assert.That(m2, Is.EqualTo(new[] {"1", "2", "2", "3", "3", "3"}));
        }
    }
}
