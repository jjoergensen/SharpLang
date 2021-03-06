// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Validation;

namespace System.Collections.Immutable
{
    /// <summary>
    /// Extension methods for immutable types.
    /// </summary>
    internal static class ImmutableExtensions
    {
        /// <summary>
        /// Tries to divine the number of elements in a sequence without actually enumerating each element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The enumerable source.</param>
        /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
        /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
        internal static bool TryGetCount<T>(this IEnumerable<T> sequence, out int count)
        {
            return TryGetCount<T>((IEnumerable)sequence, out count);
        }

        /// <summary>
        /// Tries to divine the number of elements in a sequence without actually enumerating each element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The enumerable source.</param>
        /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
        /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
        internal static bool TryGetCount<T>(this IEnumerable sequence, out int count)
        {
            var collection = sequence as ICollection;
            if (collection != null)
            {
                count = collection.Count;
                return true;
            }

            var collectionOfT = sequence as ICollection<T>;
            if (collectionOfT != null)
            {
                count = collectionOfT.Count;
                return true;
            }

            var readOnlyCollection = sequence as IReadOnlyCollection<T>;
            if (readOnlyCollection != null)
            {
                count = readOnlyCollection.Count;
                return true;
            }

            count = 0;
            return false;
        }

        /// <summary>
        /// Gets the number of elements in the specified sequence,
        /// while guaranteeing that the sequence is only enumerated once
        /// in total by this method and the caller.
        /// </summary>
        /// <typeparam name="T">The type of element in the collection.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The number of elements in the sequence.</returns>
        internal static int GetCount<T>(ref IEnumerable<T> sequence)
        {
            int count;
            if (!sequence.TryGetCount(out count))
            {
                // We cannot predict the length of the sequence. We must walk the entire sequence
                // to find the count. But avoid our caller also having to enumerate by capturing
                // the enumeration in a snapshot and passing that back to the caller.
                var list = new List<T>(sequence);
                count = list.Count;
                sequence = list;
            }

            return count;
        }

        /// <summary>
        /// Gets a copy of a sequence as an array.
        /// </summary>
        /// <typeparam name="T">The type of element.</typeparam>
        /// <param name="sequence">The sequence to be copied.</param>
        /// <param name="count">The number of elements in the sequence.</param>
        /// <returns>The array.</returns>
        /// <remarks>
        /// This is more efficient than the Enumerable.ToArray{T} extension method
        /// because that only tries to cast the sequence to ICollection{T} to determine
        /// the count before it falls back to reallocating arrays as it enumerates.
        /// </remarks>
        internal static T[] ToArray<T>(this IEnumerable<T> sequence, int count)
        {
            Requires.NotNull(sequence, "sequence");
            Requires.Range(count >= 0, "count");

            T[] array = new T[count];
            int i = 0;
            foreach (var item in sequence)
            {
                Requires.Argument(i < count);
                array[i++] = item;
            }

            Requires.Argument(i == count);
            return array;
        }

#if EqualsStructurally

        /// <summary>
        /// An optimized version of <see cref="Enumerable.SequenceEqual{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// that allows nulls, considers reference equality and count before beginning the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence1">The first sequence.</param>
        /// <param name="sequence2">The second sequence.</param>
        /// <param name="equalityComparer">The equality comparer to use for the elements.</param>
        /// <returns><c>true</c> if the sequences are equal (same elements in the same order); <c>false</c> otherwise.</returns>
        internal static bool CollectionEquals<T>(this IEnumerable<T> sequence1, IEnumerable<T> sequence2, IEqualityComparer<T> equalityComparer = null)
        {
            if (sequence1 == sequence2)
            {
                return true;
            }

            if ((sequence1 == null) ^ (sequence2 == null))
            {
                return false;
            }

            int count1, count2;
            if (sequence1.TryGetCount(out count1) && sequence2.TryGetCount(out count2))
            {
                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0 && count2 == 0)
                {
                    return true;
                }
            }

            return sequence1.SequenceEqual(sequence2, equalityComparer);
        }

        /// <summary>
        /// An optimized version of <see cref="Enumerable.SequenceEqual{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// that allows nulls, considers reference equality and count before beginning the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence1">The first sequence.</param>
        /// <param name="sequence2">The second sequence.</param>
        /// <param name="equalityComparer">The equality comparer to use for the elements.</param>
        /// <returns><c>true</c> if the sequences are equal (same elements in the same order); <c>false</c> otherwise.</returns>
        internal static bool CollectionEquals<T>(this IEnumerable<T> sequence1, IEnumerable sequence2, IEqualityComparer equalityComparer = null)
        {
            if (sequence1 == sequence2)
            {
                return true;
            }

            if ((sequence1 == null) ^ (sequence2 == null))
            {
                return false;
            }

            int count1, count2;
            if (sequence1.TryGetCount(out count1) && sequence2.TryGetCount<T>(out count2))
            {
                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0 && count2 == 0)
                {
                    return true;
                }
            }

            if (equalityComparer == null)
            {
                equalityComparer = EqualityComparer<T>.Default;
            }

            // If we have generic types we can use, use them to avoid boxing.
            var sequence2OfT = sequence2 as IEnumerable<T>;
            var equalityComparerOfT = equalityComparer as IEqualityComparer<T>;
            if (sequence2OfT != null && equalityComparerOfT != null)
            {
                return sequence1.SequenceEqual(sequence2OfT, equalityComparerOfT);
            }
            else
            {
                // We have to fallback to doing it manually since the underlying collection
                // being compared isn't a (matching) generic type.
                using (var enumerator = sequence1.GetEnumerator())
                {
                    var enumerator2 = sequence2.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            if (!enumerator2.MoveNext() || !equalityComparer.Equals(enumerator.Current, enumerator2.Current))
                            {
                                return false;
                            }
                        }

                        if (enumerator2.MoveNext())
                        {
                            return false;
                        }

                        return true;
                    }
                    finally
                    {
                        var enum2Disposable = enumerator2 as IDisposable;
                        if (enum2Disposable != null)
                        {
                            enum2Disposable.Dispose();
                        }
                    }
                }
            }
        }

#endif

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the ImmutableList&lt;T&gt;
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the ImmutableList&lt;T&gt;. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="equalityComparer">The equality comparer to use in the search.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of
        /// elements in the ImmutableList&lt;T&gt; that extends from index
        /// to the last element, if found; otherwise, �1.
        /// </returns>
        [Pure]
        public static int IndexOf<T>(this IImmutableList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            Requires.NotNull(list, "list");
            return list.IndexOf(item, 0, list.Count, equalityComparer);
        }
    }
}