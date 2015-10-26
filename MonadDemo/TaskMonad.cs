using System;
using System.Threading.Tasks;

namespace MonadDemo
{
    public static class TaskMonad
    {
        public static Task<T> Return<T>(T t)
        {
            return Task.FromResult(t);
        }

        public static Task<T2> Map<T1, T2>(this Task<T1> task, Func<T1, T2> f)
        {
            var tcs = new TaskCompletionSource<T2>();
            task.ContinueWith(_ =>
            {
                // ReSharper disable PossibleNullReferenceException
                if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
                // ReSharper restore PossibleNullReferenceException
                else if (task.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        tcs.TrySetResult(f(task.Result));
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        public static Task<T2> FlatMap<T1, T2>(this Task<T1> task, Func<T1, Task<T2>> f)
        {
            var tcs = new TaskCompletionSource<T2>();
            task.ContinueWith(_ =>
            {
                // ReSharper disable PossibleNullReferenceException
                if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
                // ReSharper restore PossibleNullReferenceException
                else if (task.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        var t = f(task.Result);
                        if (t == null) tcs.TrySetCanceled();
                        else
                            t.ContinueWith(__ =>
                            {
                                // ReSharper disable PossibleNullReferenceException
                                if (t.IsFaulted) tcs.TrySetException(t.Exception.InnerExceptions);
                                // ReSharper restore PossibleNullReferenceException
                                else if (t.IsCanceled) tcs.TrySetCanceled();
                                else tcs.TrySetResult(t.Result);
                            }, TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        public static Task<T2> Select<T1, T2>(this Task<T1> m, Func<T1, T2> f)
        {
            return m.Map(f);
        }

        public static Task<T2> SelectMany<T1, T2>(this Task<T1> m, Func<T1, Task<T2>> f)
        {
            return m.FlatMap(f);
        }

        public static Task<T3> SelectMany<T1, T2, T3>(this Task<T1> m, Func<T1, Task<T2>> f1, Func<T1, T2, T3> f2)
        {
            return m.FlatMap(t1 =>
                f1(t1).Map(t2 =>
                    f2(t1, t2)));
        }
    }
}
