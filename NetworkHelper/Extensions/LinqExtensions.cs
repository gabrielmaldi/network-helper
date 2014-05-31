using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetworkHelper.Utilities;

namespace NetworkHelper.Extensions
{
    /// <summary>
    /// Contains extension methods for System.Collections.Generic.IEnumerable
    /// </summary>
    public static class LinqExtensions
    {
        private static readonly Lazy<MethodInfo> _OrderByMethodInfo = new Lazy<MethodInfo>(() => typeof(Enumerable).GetMethods().Single(method => method.Name == "OrderBy" && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 3));
        private static readonly Lazy<MethodInfo> _OrderByDescendingMethodInfo = new Lazy<MethodInfo>(() => typeof(Enumerable).GetMethods().Single(method => method.Name == "OrderByDescending" && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 3));
        private static readonly Lazy<MethodInfo> _ThenByMethodInfo = new Lazy<MethodInfo>(() => typeof(Enumerable).GetMethods().Single(method => method.Name == "ThenBy" && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 3));
        private static readonly Lazy<MethodInfo> _ThenByDescendingMethodInfo = new Lazy<MethodInfo>(() => typeof(Enumerable).GetMethods().Single(method => method.Name == "ThenByDescending" && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 3));

        public static IEnumerable<T> Empty<T>(T type)
        {
            return Enumerable.Empty<T>();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return new HashSet<T>(source, comparer);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return new HashSet<T>(source, LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new HashSet<T>(source);
        }

        public static List<T> TryToList<T>(this IEnumerable<T> source)
        {
            return source != null ? source.ToList() : null;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> hasher, params T[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            bool result = false;

            LambdaEqualityComparer<T> lambdaEqualityComparer = LambdaEqualityComparer.Create(comparer, hasher);

            foreach (T value in values)
            {
                if (source.Contains(value, lambdaEqualityComparer))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether no element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable &lt; TSource &gt; that contains the elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if no element of the source sequence passes the test in the specified predicate, or if the sequence is empty; otherwise, false.</returns>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return !source.Any(predicate);
        }

        /// <summary>
        /// Determines whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable &lt; TSource &gt; to check for emptiness.</param>
        /// <returns>true if the source sequence contains no elements; otherwise, false.</returns>
        public static bool None<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            
            return !source.Any();
        }

        public static bool HasDuplicates<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return source.HasDuplicates(LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static bool HasDuplicates<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.HasDuplicates(null);
        }

        private static bool HasDuplicates<T>(this IEnumerable<T> source, LambdaEqualityComparer<T> lambdaEqualityComparer)
        {
            bool result = false;

            HashSet<T> hashSet = lambdaEqualityComparer != null ? new HashSet<T>(lambdaEqualityComparer) : new HashSet<T>();
            foreach (T element in source)
            {
                if (!hashSet.Add(element))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return source.Distinct(LambdaEqualityComparer.Create(comparer, hasher));
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <param name="comparer">An System.Collections.Generic.IComparer &lt; TKey &gt; to compare keys.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            return !descending ? source.OrderBy(keySelector, comparer) : source.OrderByDescending(keySelector, comparer);
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            return !descending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <param name="comparer">An System.Collections.IComparer to compare keys.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string keyPropertyName, IComparer comparer, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.Sort(keyPropertyName, comparer, descending, false);
        }

        /// <summary>
        /// Sorts the elements of a sequence according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string keyPropertyName, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.OrderBy(keyPropertyName, null, descending);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string keyPropertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.OrderBy(keyPropertyName, false);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <param name="comparer">An System.Collections.Generic.IComparer &lt; TKey &gt; to compare keys.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            return !descending ? source.ThenBy(keySelector, comparer) : source.ThenByDescending(keySelector, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            return !descending ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <param name="comparer">An System.Collections.IComparer to compare keys.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string keyPropertyName, IComparer comparer, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.Sort(keyPropertyName, comparer, descending, true);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string keyPropertyName, bool descending)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.ThenBy(keyPropertyName, null, descending);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in according to a key
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keyPropertyName">The name of the property to use as the key.</param>
        /// <returns>An System.Linq.IOrderedEnumerable &lt; TSource &gt; whose elements are sorted according to a key</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string keyPropertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(keyPropertyName))
            {
                throw new ArgumentNullException("keyPropertyName");
            }

            return source.ThenBy(keyPropertyName, false);
        }

        private static IOrderedEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> source, string keyPropertyName, IComparer comparer, bool descending, bool thenBy)
        {
            IOrderedEnumerable<TSource> result;

            string[] propertiesNames = keyPropertyName.Split('.');
            Type type = typeof(TSource);
            ParameterExpression parameter = Expression.Parameter(type);
            Expression expression = parameter;
            foreach (string propertyName in propertiesNames)
            {
                PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (property != null && property.CanRead)
                {
                    expression = Expression.Property(expression, property);
                    type = property.PropertyType;
                }
                else
                {
                    expression = null;
                    type = null;
                    break;
                }
            }

            if (expression != null && type != null)
            {
                Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), type);
                LambdaExpression lambda = Expression.Lambda(delegateType, expression, parameter);

                MethodInfo method;
                if (thenBy)
                {
                    if (descending)
                    {
                        method = _ThenByDescendingMethodInfo.Value;
                    }
                    else
                    {
                        method = _ThenByMethodInfo.Value;
                    }
                }
                else
                {
                    if (descending)
                    {
                        method = _OrderByDescendingMethodInfo.Value;
                    }
                    else
                    {
                        method = _OrderByMethodInfo.Value;
                    }
                }
                method = method.MakeGenericMethod(typeof(TSource), type);
                
                result = (IOrderedEnumerable<TSource>)method.Invoke(null, new object[] { source, lambda.Compile(), comparer });
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type \"{0}\" does not contain a property \"{1}\"", typeof(TSource).FullName, keyPropertyName));
            }

            return result;
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return first.Union(second, LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return first.Intersect(second, LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer, Func<T, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return first.Except(second, LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, Func<TKey, TKey, bool> comparer, Func<TKey, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, LambdaEqualityComparer.Create(comparer, hasher));
        }

        public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKeySelector, Func<TRight, TKey> rightKeySelector, Func<TLeft, TRight, TResult> resultSelector, Func<TKey, TKey, bool> comparer, Func<TKey, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            var groupJoin = left.GroupJoin(right, leftKeySelector, rightKeySelector, (leftElement, rightElements) => new { LeftElement = leftElement, RightElements = rightElements }, LambdaEqualityComparer.Create(comparer, hasher));
            return groupJoin.SelectMany(joinedElement => joinedElement.RightElements.DefaultIfEmpty(), (joinedElement, rightElement) => resultSelector(joinedElement.LeftElement, rightElement));
        }

        public static IEnumerable<TResult> RightJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKeySelector, Func<TRight, TKey> rightKeySelector, Func<TLeft, TRight, TResult> resultSelector, Func<TKey, TKey, bool> comparer, Func<TKey, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            return right.LeftJoin(left, rightKeySelector, leftKeySelector, (rightElement, leftElement) => resultSelector(leftElement, rightElement), comparer, hasher);
        }

        public static IEnumerable<TResult> FullJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKeySelector, Func<TRight, TKey> rightKeySelector, Func<TLeft, TRight, TResult> resultSelector, Func<TKey, TKey, bool> comparer, Func<TKey, int> hasher)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            var leftJoin = left.LeftJoin(right, leftKeySelector, rightKeySelector, (leftElement, rightElement) => new { LeftElement = leftElement, RightElement = rightElement }, comparer, hasher);
            var rightJoin = left.RightJoin(right, leftKeySelector, rightKeySelector, (leftElement, rightElement) => new { LeftElement = leftElement, RightElement = rightElement }, comparer, hasher);
            var union = leftJoin.Union(rightJoin);
            return union.Select(joinedElement => resultSelector(joinedElement.LeftElement, joinedElement.RightElement));
        }

        public static IEnumerable<IEnumerable<T>> CrossJoin<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            IEnumerable<IEnumerable<T>> empty = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(empty, (accumulator, sequence) => accumulator.SelectMany(_ => sequence, (accumulatedSequence, element) => accumulatedSequence.Concat(new[] { element })));
        }

        public static IEnumerable<IEnumerable<T>> CrossJoin<T>(this IEnumerable<T> first, IEnumerable<T> second, params IEnumerable<T>[] rest)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            IEnumerable<T>[] sequences;

            if (rest == null)
            {
                sequences = new[] { first, second };
            }
            else
            {
                sequences = new IEnumerable<T>[2 + rest.Length];
                sequences[0] = first;
                sequences[1] = second;
                for (int i = 0; i < rest.Length; i++)
                {
                    sequences[2 + i] = rest[i];
                }
            }

            return sequences.CrossJoin();
        }
    }
}