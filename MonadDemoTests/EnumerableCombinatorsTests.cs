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
                //new[] {1, 4}.AsEnumerable(),
                //new[] {1, 5}.AsEnumerable(),
                //new[] {1, 6}.AsEnumerable(),
                //new[] {2, 4}.AsEnumerable(),
                //new[] {2, 5}.AsEnumerable(),
                //new[] {2, 6}.AsEnumerable(),
                //new[] {3, 4}.AsEnumerable(),
                //new[] {3, 5}.AsEnumerable(),
                //new[] {3, 6}.AsEnumerable()
                new[] {1, 4}.AsEnumerable(),
                new[] {2, 4}.AsEnumerable(),
                new[] {3, 4}.AsEnumerable(),
                new[] {1, 5}.AsEnumerable(),
                new[] {2, 5}.AsEnumerable(),
                new[] {3, 5}.AsEnumerable(),
                new[] {1, 6}.AsEnumerable(),
                new[] {2, 6}.AsEnumerable(),
                new[] {3, 6}.AsEnumerable()
            }.AsEnumerable();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Sequence2()
        {
            var ms = new[]
            {
                new[] {1, 2}.AsEnumerable(),
                new[] {3, 4}.AsEnumerable()
            }.AsEnumerable();

            var actual = ms.Sequence();

            var expected = new[]
            {
                //new[] {1, 3}.AsEnumerable(),
                //new[] {1, 4}.AsEnumerable(),
                //new[] {2, 3}.AsEnumerable(),
                //new[] {2, 4}.AsEnumerable(),
                new[] {1, 3}.AsEnumerable(),
                new[] {2, 3}.AsEnumerable(),
                new[] {1, 4}.AsEnumerable(),
                new[] {2, 4}.AsEnumerable(),
            }.AsEnumerable();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
