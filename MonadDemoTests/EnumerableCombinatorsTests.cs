using System;
using System.Linq;
using MonadDemo;
using NUnit.Framework;

namespace MonadDemoTests
{
    [TestFixture]
    public class EnumerableCombinatorsTests
    {
        [Test]
        public void Sequence()
        {
            var ms = new[]
            {
                new[] {1, 2, 3}.AsEnumerable(),
                new[] {4, 5, 6}.AsEnumerable()
            }.AsEnumerable();

            var actual = ms.Sequence();

            var expected = new[]
            {
                new[] {1, 4}.AsEnumerable(),
                new[] {1, 5}.AsEnumerable(),
                new[] {1, 6}.AsEnumerable(),
                new[] {2, 4}.AsEnumerable(),
                new[] {2, 5}.AsEnumerable(),
                new[] {2, 6}.AsEnumerable(),
                new[] {3, 4}.AsEnumerable(),
                new[] {3, 5}.AsEnumerable(),
                new[] {3, 6}.AsEnumerable()
            }.AsEnumerable();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void LiftM2()
        {
            var m1 = new[] {1, 2, 3}.AsEnumerable();
            var m2 = new[] {4, 5, 6}.AsEnumerable();
            Func<int, int, int> f = (a, b) => a + b;
            var m3 = EnumerableCombinators.LiftM2(m1, m2, f);
            Assert.That(m3, Is.EqualTo(new[]
            {
                1 + 4, 1 + 5, 1 + 6,
                2 + 4, 2 + 5, 2 + 6,
                3 + 4, 3 + 5, 3 + 6
            }));
        }
    }
}
