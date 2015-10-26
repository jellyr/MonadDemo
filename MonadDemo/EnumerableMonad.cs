using System;
using System.Collections.Generic;
using System.Linq;

namespace MonadDemo
{
    public static class EnumerableMonad
    {
        public static IEnumerable<T> Return<T>(T t)
        {
            yield return t;
        }

        public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> m, Func<T1, T2> f)
        {
            return m.Select(f);
        }

        public static IEnumerable<T2> FlatMap<T1, T2>(this IEnumerable<T1> m, Func<T1, IEnumerable<T2>> f)
        {
            return m.SelectMany(f);
        }
    }
}
