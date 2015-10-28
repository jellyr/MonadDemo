using System;
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

        [Test]
        public void LiftM2()
        {
            var m1 = Task.Factory.StartNew(() => { Thread.Sleep(100); return 1; });
            var m2 = Task.Factory.StartNew(() => { Thread.Sleep(200); return 2; });
            Func<int, int, int> f = (a, b) => a + b;
            var m3 = TaskCombinators.LiftM2(m1, m2, f);
            Assert.That(m3.Result, Is.EqualTo(3));
        }
    }
}
