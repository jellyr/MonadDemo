using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MonadDemo
{
    public static class EnumerableExtensions
    {
        public static TAccumulate FoldRight<T, TAccumulate>(
            this IEnumerable<T> xs,
            TAccumulate z,
            Func<T, TAccumulate, TAccumulate> f)
        {
            using (var e = xs.GetEnumerator()) return FoldRightHelper(f, z, e);
        }

        private static TAccumulate FoldRightHelper<T, TAccumulate>(
            Func<T, TAccumulate, TAccumulate> f,
            TAccumulate z,
            IEnumerator<T> e)
        {
            // if the list is empty, the result is the initial value z; else
            // apply f to the first element and the result of folding the rest
            // foldr _ z [] = z
            // foldr f z (x:xs) = f x (foldr f z xs)

            if (!e.MoveNext()) return z;

            var x = e.Current;
            var xs = e;

            return f(x, FoldRightHelper(f, z, xs));
        }
    }
}
