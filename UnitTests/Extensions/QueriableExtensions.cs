using System;
using System.Linq;

namespace UnitTests.Extensions
{
    public static class QueriableExtensions
    {
        /// <summary>
        /// Apply where predicate depending on condition
        /// </summary>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Func<T, bool> predicate)
        {
            if (condition)
                query = query.Where(predicate).AsQueryable();

            return query;
        }
    }
}
