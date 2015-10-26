using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MonadDemo
{
    public static class EnumerableCombinators
    {
        public static IEnumerable<IEnumerable<T>> Sequence<T>(this IEnumerable<IEnumerable<T>> ms)
        {
            var seed = EnumerableMonad.Return(ImmutableList<T>.Empty);
            var result = ms.Aggregate(seed, (acc, m) =>
                m.FlatMap(t =>
                    acc.FlatMap(ts =>
                        EnumerableMonad.Return(ts.Add(t)))));
            return result.Map(immutableList => immutableList.AsEnumerable());
        }

        public static IEnumerable<T3> LiftM2<T1, T2, T3>(IEnumerable<T1> xs, IEnumerable<T2> ys, Func<T1, T2, T3> f)
        {
            return xs.SelectMany(a =>
                ys.SelectMany(b =>
                    EnumerableMonad.Return(f(a, b))));
        }
    }
}
