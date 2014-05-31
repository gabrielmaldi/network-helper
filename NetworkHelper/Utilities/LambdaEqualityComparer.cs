using System;
using System.Collections.Generic;

namespace NetworkHelper.Utilities
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hasher;

        public LambdaEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            _comparer = comparer;
            _hasher = hasher;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hasher(obj);
        }
    }

    public static class LambdaEqualityComparer
    {
        public static LambdaEqualityComparer<T> Create<T>(Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            return new LambdaEqualityComparer<T>(comparer, hasher);
        }
    }
}