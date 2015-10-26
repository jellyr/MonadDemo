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
            var seed = TaskMonad.Return(ImmutableList<T>.Empty);
            var result = ms.Aggregate(seed, (acc, m) =>
                m.FlatMap(t =>
                    acc.FlatMap(ts =>
                        TaskMonad.Return(ts.Add(t)))));
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
