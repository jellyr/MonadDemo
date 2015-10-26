using System;
using System.Collections.Generic;
using System.Linq;

namespace MonadDemo
{
    public static class EnumerableCombinators
    {
        public static IEnumerable<IEnumerable<T>> Sequence<T>(this IEnumerable<IEnumerable<T>> ms)
        {
            var seed = EnumerableMonad.Return(Enumerable.Empty<T>());
            return ms.Aggregate(
                seed, (acc, m) => m.SelectMany(t =>
                    acc.SelectMany(ts =>
                        EnumerableMonad.Return(ts.Concat(EnumerableMonad.Return(t))))));
        }

        public static IEnumerable<T3> LiftM2<T1, T2, T3>(IEnumerable<T1> xs, IEnumerable<T2> ys, Func<T1, T2, T3> f)
        {
            return xs.SelectMany(a =>
                ys.SelectMany(b =>
                    EnumerableMonad.Return(f(a, b))));
        }
    }
}
