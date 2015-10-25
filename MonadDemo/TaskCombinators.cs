using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonadDemo
{
    public static class TaskCombinators
    {
        public static Task<IEnumerable<T>> Sequence<T>(IEnumerable<Task<T>> sequence)
        {
            //var monadAdapter = MonadAdapterRegistry.Get(typeof(TMonad));
            //var z = monadAdapter.Return(MonadHelpers.Nil<TA>());
            //return (TMonad)ms.FoldRight(
            //    z, (m, mtick) => monadAdapter.Bind(
            //        m, x => monadAdapter.Bind(
            //            mtick, xs => monadAdapter.Return(MonadHelpers.Cons(x, xs)))));

            // We could reverse the sequence and then call Aggregate aka FoldLeft.
            // Or implement FoldRight (see Flinq).

            return null;
        }

        public static Task<T3> LiftM2<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Func<T1, T2, T3> f)
        {
            return task1.FlatMap(a =>
                task2.FlatMap(b =>
                    TaskMonad.Return(f(a, b))));
        }
    }
}
