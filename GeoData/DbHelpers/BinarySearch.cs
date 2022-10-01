using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.DbHelpers
{
    public class BinarySearch
    {
        public static int SearchLeftmost<T>(T needle, Func<int, T, int> Compare, int recordsCount)
        {
            int left = 0;
            int right = recordsCount;

            while (left < right)
            {
                var m = (left + right) / 2;
                var res = Compare(m, needle);
                if (res > 0)
                    left = m + 1;
                else
                    right = m;
            }
            return left;
        }

        public static int? Search<T>(T needle, Func<int, T, int> Compare, int recordsCount)
        {
            int left = 0;
            int right = recordsCount - 1;

            while (left <= right)
            {
                var m = (left + right) / 2;
                var res = Compare(m, needle);
                if (res > 0)
                    left = m + 1;
                else if (res < 0)
                    right = m - 1;
                else
                    return m;
            }
            return null;
        }
    }
}
