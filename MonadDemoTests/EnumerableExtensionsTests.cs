using System.Linq;
using MonadDemo;
using NUnit.Framework;

namespace MonadDemoTests
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void FoldRight1()
        {
            var xs = new[] {1,2,3};
            var actual = xs.FoldRight(0, (a, b) => a + b);
            Assert.That(actual, Is.EqualTo(6));
        }

        [Test]
        public void FoldRight2()
        {
            var xs = new[] {"A", "B", "C"};
            var actual = xs.FoldRight("", (a, b) => a + b);
            Assert.That(actual, Is.EqualTo("ABC"));
        }

        [Test]
        public void FoldRight3()
        {
            var xs = new[]
            {
                new[] {1, 2, 3}.AsEnumerable(),
                new[] {4, 5, 6}.AsEnumerable(),
                new[] {7, 8, 9}.AsEnumerable()
            }.AsEnumerable();
            var actual = xs.FoldRight(Enumerable.Empty<int>(), Enumerable.Concat);
            Assert.That(actual, Is.EqualTo(new[] {1,2,3,4,5,6,7,8,9}));
        }
    }
}
