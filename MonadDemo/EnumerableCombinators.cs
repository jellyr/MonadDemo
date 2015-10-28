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

            //var result = ms.Aggregate(seed, (acc, m) =>
            //    m.FlatMap(t =>
            //        acc.FlatMap(ts =>
            //            EnumerableMonad.Return(ts.Add(t)))));

            //var result = ms.Aggregate(seed, (acc, m) => m.FlatMap(t => acc.Map(ts => ts.Add(t))));

            // e.g. ms = [[1,2],[3,4]]
            var result = ms.Aggregate(seed, (acc, m) =>
                // e.g. m = [1,2]
                m.SelectMany(t =>
                // do the following with t = 1 then t = 2
                {
                    // initially, acc is [[]]
                    // [[]] => [[1]]
                    var v1 = acc.Select(ts =>
                    {
                        var v2 = ts.Add(t);
                        return v2;
                    });
                    return v1;
                }));

            //var result = ms.Aggregate(seed, (acc, m) =>
            //    from t in m
            //    from ts in acc
            //    select ts.Add(t));

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
