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
            Func<IEnumerable<T>, IEnumerable<ImmutableList<T>>, IEnumerable<ImmutableList<T>>> k = (m, acc) =>
                from t in m
                from ts in acc
                select ts.Insert(0, t);

            var z = EnumerableMonad.Return(ImmutableList<T>.Empty);
            var result = ms.FoldRight(z, k);

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
