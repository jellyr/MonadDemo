using System;
using System.Threading;
using System.Threading.Tasks;
using MonadDemo;
using NUnit.Framework;

namespace MonadDemoTests
{
    [TestFixture]
    public class TaskMonadTests
    {
        [Test]
        public void TaskMapTrivial()
        {
            var t1 = 5.Return();
            var t2 = t1.Map(Convert.ToString);
            Assert.That(t2.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskMapRealistic()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                MimicRealisticProcessing();
                return 5;
            });
            var t2 = t1.Map(Convert.ToString);
            Assert.That(t2.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskMapExceptionInFirstTask()
        {
            var t1 = Task<int>.Factory.StartNew(() =>
            {
                throw new DivideByZeroException();
            });
            var t2 = t1.Map(Convert.ToString);
            var ex = Assert.Throws<AggregateException>(() => t2.Wait());
            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.InstanceOf<DivideByZeroException>());
            Assert.That(ex.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(ex.InnerExceptions[0], Is.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void TaskMapExceptionInMappingFunction()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                MimicRealisticProcessing();
                return 5;
            });
            var t2 = t1.Map<int, string>(_ =>
            {
                throw new DivideByZeroException();
            });
            var ex = Assert.Throws<AggregateException>(() => t2.Wait());
            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.InstanceOf<DivideByZeroException>());
            Assert.That(ex.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(ex.InnerExceptions[0], Is.InstanceOf<DivideByZeroException>());
        }

        [Test]
        public void TaskMapWhenTaskIsCancelledBeforeItStarted()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var cancellationToken = cancellationTokenSource.Token;
            var t1 = Task.Factory.StartNew(() =>
            {
                MimicRealisticProcessing();
                return 5;
            }, cancellationToken);
            var t2 = t1.Map(Convert.ToString);
            var ex = Assert.Throws<AggregateException>(() => t2.Wait(CancellationToken.None));
            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.InstanceOf<TaskCanceledException>());
            Assert.That(ex.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(ex.InnerExceptions[0], Is.InstanceOf<TaskCanceledException>());
            Assert.That(t2.IsCanceled, Is.True);
        }

        [Test]
        public void TaskMapWhenTaskIsCancelledAfterItStarted()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var t1 = Task.Factory.StartNew(() =>
            {
                MimicRealisticProcessing();
                cancellationToken.ThrowIfCancellationRequested();
                return 5;
            }, cancellationToken);
            var t2 = t1.Map(Convert.ToString);
            for (;;)
            {
                // ReSharper disable once InvertIf
                if (t1.Status == TaskStatus.Running)
                {
                    cancellationTokenSource.Cancel();
                    break;
                }
            }
            cancellationTokenSource.Cancel();
            var ex = Assert.Throws<AggregateException>(() => t2.Wait(CancellationToken.None));
            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.InstanceOf<TaskCanceledException>());
            Assert.That(ex.InnerExceptions.Count, Is.EqualTo(1));
            Assert.That(ex.InnerExceptions[0], Is.InstanceOf<TaskCanceledException>());
            Assert.That(t2.IsCanceled, Is.True);
        }

        [Test]
        public void TaskFlatMapTrivial()
        {
            var t1 = Task.FromResult(5);
            var t2 = t1.FlatMap(n => Convert.ToString(n).Return());
            Assert.That(t2.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskFlatMapRealistic()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                MimicRealisticProcessing();
                return 5;
            });
            var t2 = t1.FlatMap(n => Convert.ToString(n).Return());
            Assert.That(t2.Result, Is.EqualTo("5"));
        }

        // TODO: FlatMap/exception
        // TODO: FlatMap/cancellation

        [Test]
        public void TaskSelect()
        {
        }

        [Test]
        public void TaskSelectMany()
        {
        }

        [Test]
        public void TaskSelectManyOverload()
        {
        }

        private static void MimicRealisticProcessing()
        {
            // Busy wait for a few hundred milliseconds.
            Thread.SpinWait(100000000);
        }
    }
}
