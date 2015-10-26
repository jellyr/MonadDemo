using System.Threading;
using System.Threading.Tasks;
using MonadDemo;
using NUnit.Framework;

namespace MonadDemoTests
{
    [TestFixture]
    public class TaskCombinatorsTests
    {
        [Test]
        public void Sequence()
        {
            var ms = new[]
            {
                Task.Factory.StartNew(() => { Thread.Sleep(100); return 1; }),
                Task.Factory.StartNew(() => { Thread.Sleep(200); return 2; }),
                Task.Factory.StartNew(() => { Thread.Sleep(300); return 3; })
            };

            var actual = ms.Sequence();

            Assert.That(actual.Result, Is.EqualTo(new[] {1, 2, 3}));
        }
    }
}
