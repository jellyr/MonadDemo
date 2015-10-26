using System.Collections.Generic;

namespace MonadDemo
{
    public static class EnumerableMonad
    {
        public static IEnumerable<T> Return<T>(T t)
        {
            yield return t;
        }
    }
}
