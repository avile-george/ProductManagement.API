using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsApp.Application.Helpers
{
    public static class LevenshteinDistanceHelper
    {
        // 1. Core Levenshtein Algorithm - Zero heap allocations
        public static int GetLevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return string.IsNullOrEmpty(target) ? 0 : target.Length;
            if (string.IsNullOrEmpty(target)) return source.Length;

            // Ensure source is the shorter string for space optimization
            if (source.Length > target.Length)
            {
                (source, target) = (target, source);
            }

            source = source.ToLowerInvariant();
            target = target.ToLowerInvariant();

            int[] d = new int[source.Length + 1];

            for (int i = 0; i <= source.Length; i++) d[i] = i;

            for (int j = 1; j <= target.Length; j++)
            {
                int previousVal = d[0];
                d[0] = j;

                for (int i = 1; i <= source.Length; i++)
                {
                    int temp = d[i];
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    d[i] = Math.Min(
                        Math.Min(d[i - 1] + 1, d[i] + 1),
                        previousVal + cost
                    );

                    previousVal = temp;
                }
            }

            return d[source.Length];
        }
    }
}
