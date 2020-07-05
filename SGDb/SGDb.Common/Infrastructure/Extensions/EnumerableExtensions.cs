using System;
using System.Collections.Generic;
using System.Linq;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> callback)
        {
            foreach (var element in source)
            {
                callback(element);
            }
        }
    }
}