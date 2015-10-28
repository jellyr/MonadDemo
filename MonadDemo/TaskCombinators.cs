using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace MonadDemo
{
    public static class TaskCombinators
    {
        public static Task<IEnumerable<T>> Sequence<T>(this IEnumerable<Task<T>> ms)
        {
            Func<Task<T>, Task<ImmutableList<T>>, Task<ImmutableList<T>>> k = (m, acc) =>
                from t in m
                from ts in acc
                select ts.Insert(0, t);

            var z = TaskMonad.Return(ImmutableList<T>.Empty);
            var result = ms.FoldRight(z, k);

            return result.Map(immutableList => immutableList.AsEnumerable());
        }

        public static Task<T3> LiftM2<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Func<T1, T2, T3> f)
        {
            return task1.FlatMap(a =>
                task2.FlatMap(b =>
                    TaskMonad.Return(f(a, b))));
        }
    }
}
