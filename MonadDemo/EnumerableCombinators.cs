using System;
using System.Collections.Generic;
using System.Linq;

namespace MonadDemo
{
    public static class EnumerableCombinators
    {
        public static IEnumerable<IEnumerable<T>> Sequence<T>(IEnumerable<IEnumerable<T>> sequence)
        {
            return null;
        }

        public static IEnumerable<T3> LiftM2<T1, T2, T3>(IEnumerable<T1> xs, IEnumerable<T2> ys, Func<T1, T2, T3> f)
        {
            return xs.SelectMany(a =>
                ys.SelectMany(b =>
                    EnumerableMonad.Return(f(a, b))));
        }
    }
}
