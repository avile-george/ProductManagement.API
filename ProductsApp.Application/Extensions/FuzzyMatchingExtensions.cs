using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Application.Helpers;

namespace ProductsApp.Application.Extensions
{
    public static class FuzzyMatchingExtensions
    {
        // Finds the single best match based on a specified property and distance threshold
        public static T? FindBestMatch<T>(this IEnumerable<T> items, string query, Func<T, string> propertySelector, int maxDistance = 3)
        {
            T? bestMatch = default;
            int minDistance = int.MaxValue;

            foreach (var item in items)
            {
                string propertyValue = propertySelector(item);
                if (string.IsNullOrEmpty(propertyValue)) continue;

                int distance = LevenshteinDistanceHelper.GetLevenshteinDistance(query, propertyValue);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestMatch = item;

                    if (minDistance == 0) break; // Exact match found
                }
            }

            return minDistance <= maxDistance ? bestMatch : default;
        }

        // Finds ALL items where any specified property meets the distance threshold
        public static IEnumerable<T> FindAllMatches<T>(
            this IEnumerable<T> items,
            string query,
            int maxDistance,
            params Func<T, string>[] propertySelectors)
        {
            if (items == null || string.IsNullOrEmpty(query)) yield break;

            var results = new List<Tuple<T, int>>();

            Parallel.ForEach(items, item =>
            {
                foreach (var selector in propertySelectors)
                {
                    string propertyValue = selector(item);
                    if (string.IsNullOrEmpty(propertyValue)) continue;

                    int distance;

                    if (propertyValue.Equals(query, StringComparison.OrdinalIgnoreCase))
                    {
                        lock (results)
                        {
                            results.Add(Tuple.Create(item, 0)); // Exact match
                        }
                        break;
                    }
                    else if (propertyValue.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    {
                        lock (results)
                        {
                            results.Add(Tuple.Create(item, 1)); // Prefix match
                        }
                        break;
                    }

                    // 1. Performance Guard: Skip Levenshtein if length difference is too large
                    if (Math.Abs(query.Length - propertyValue.Length) > maxDistance) continue;

                    // 2. Calc Distance
                    distance = LevenshteinDistanceHelper.GetLevenshteinDistance(query, propertyValue);

                    // 3. Match found for this item; return it and skip its remaining fields
                    if (distance <= maxDistance)
                    {
                        lock (results)
                        {
                            results.Add(Tuple.Create(item, distance));
                        }
                        break;
                    }
                }
            });

            foreach (var result in results.OrderBy(r => r.Item2).ToList()) //4. Sort by distance (best matches first)
            {
                yield return result.Item1;
            }
        }
    }
}
