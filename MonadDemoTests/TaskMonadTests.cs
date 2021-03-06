﻿using System;
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
        public void Return()
        {
            var m = TaskMonad.Return(5);
            Assert.That(m.Result, Is.EqualTo(5));
        }

        [Test]
        public void TaskMapTrivial()
        {
            var t1 = TaskMonad.Return(5);
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
            var t2 = t1.FlatMap(n => TaskMonad.Return(Convert.ToString(n)));
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
            var t2 = t1.FlatMap(n => TaskMonad.Return(Convert.ToString(n)));
            Assert.That(t2.Result, Is.EqualTo("5"));
        }

        // TODO: FlatMap/exception
        // TODO: FlatMap/cancellation

        [Test]
        public void UsingMapToTryToMimicFlatMap()
        {
            var t1 = TaskMonad.Return(5);
            var t2 = t1.Map(n => TaskMonad.Return(Convert.ToString(n)));
            // t2 is Task<Task<string>> instead of Task<string>.
            Assert.That(t2.Result.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskSelectMethodChain()
        {
            var m = TaskMonad.Return(5).Select(Convert.ToString);
            Assert.That(m.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskSelectLinq()
        {
            var m =
                from t in TaskMonad.Return(5)
                select Convert.ToString(t);
            Assert.That(m.Result, Is.EqualTo("5"));
        }

        [Test]
        public void TaskSelectManyMethodChain()
        {
            var m = TaskMonad.Return(5).SelectMany(t1 =>
                TaskMonad.Return(t1 * 10));
            Assert.That(m.Result, Is.EqualTo(50));
        }

        [Test]
        public void TaskSelectManyLinq()
        {
            var m =
                from t1 in TaskMonad.Return(5)
                from t2 in TaskMonad.Return(t1 * 10)
                select t2;
            Assert.That(m.Result, Is.EqualTo(50));
        }

        [Test]
        public void TaskSelectManyOverloadMethodChain()
        {
            var m = TaskMonad.Return(5).SelectMany(t1 =>
                TaskMonad.Return(10), (t1, t2) =>
                    Convert.ToString(t1 * t2));
            Assert.That(m.Result, Is.EqualTo("50"));
        }

        [Test]
        public void TaskSelectManyOverloadLinq()
        {
            var m =
                from t1 in TaskMonad.Return(5)
                from t2 in TaskMonad.Return(10)
                select Convert.ToString(t1 * t2);
            Assert.That(m.Result, Is.EqualTo("50"));
        }

        private static void MimicRealisticProcessing()
        {
            // Busy wait for a few hundred milliseconds.
            Thread.SpinWait(100000000);
        }
    }
}
