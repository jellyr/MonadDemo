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
    }
}
