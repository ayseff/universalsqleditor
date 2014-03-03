using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Collections
{
    /// <summary>
    /// Class that provides extension methods for Collections classes.
    /// </summary>
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Adds a range of objects to a list.
        /// </summary>
        /// <typeparam name="T">Type of object being added.</typeparam>
        /// <param name="list">List where objects are added.</param>
        /// <param name="objects">List of objects to add.</param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> objects)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (objects == null) throw new ArgumentNullException("objects");
            foreach (var o in objects)
            {
                list.Add(o);
            }
        }

        /// <summary>
        /// Determines whether a list is empty.
        /// </summary>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <param name="list">List to check.</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Determines whether a list is empty.
        /// </summary>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <param name="list">List to check.</param>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Any();
        }

        /// <summary>
        /// Removes all items from a collection.
        /// </summary>
        /// <typeparam name="T">Type of items stored in the collection.</typeparam>
        /// <param name="list">Collection from which to remove items.</param>
        /// <param name="itemsToRemove">Items to remove.</param>
        /// <returns><code>true</code> if all items were successfully removed, <code>false</code> otherwise.</returns>
        public static bool RemoveAll<T>(this ICollection<T> list, IEnumerable<T> itemsToRemove)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (itemsToRemove == null) throw new ArgumentNullException("itemsToRemove");

            var removalSuccessful = true;
            foreach (var item in itemsToRemove)
            {
                removalSuccessful = removalSuccessful && list.Remove(item);
            }
            return removalSuccessful;
        }
    }
}
