using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.DbHelpers
{
    public class BinarySearch
    {
        /// <summary>
        /// move search range to upper part of the uncheked data. 
        /// min become previous position, position shifts to middle of min-max range
        /// </summary>
        /// <param name="max"></param>
        /// <param name="position"></param>
        /// <param name="min"></param>
        public static void BinaryStepUp(int max, ref int position, ref int min)
        {
            min = position;
            position += (max - position) / 2;
        }

        /// <summary>
        /// move search range to lower part of the unchecked data. 
        /// max become previous position, position shifts to middle of min-max range
        /// </summary>
        /// <param name="max"></param>
        /// <param name="position"></param>
        /// <param name="min"></param>
        public static void BinaryStepDown(ref int max, ref int position, int min)
        {
            max = position;
            position -= (position - min) / 2;
        }

        public static IEnumerable<int> SearchMany<T>(T needle, Func<int, T, int> Compare, int recordsCount)
        {
            int max = recordsCount;
            int position = recordsCount / 2;
            int min = 0;

            //lets constrain iterations just in case
            int maxIterations = recordsCount;
            while (maxIterations > 0)
            {
                maxIterations--;

                var res = Compare(position, needle);
                if (res == 0)
                {
                    //we get to a point that fits, but we may need to move to top of the list of fitting entries.
                    if (position > 0 && Compare(position - 1, needle) == 0)
                    {
                        DbHelpers.BinarySearch.BinaryStepDown(ref max, ref position, min);
                        continue;
                    }
                    //we get to top of list lets return all the cities
                    do
                    {
                        yield return position;
                        position++;
                    }
                    while (position < recordsCount && Compare(position, needle) == 0);
                    //all cities are returned stop the enumeration
                    break;
                }
                else if (res > 0)
                {
                    DbHelpers.BinarySearch.BinaryStepUp(max, ref position, ref min);
                    continue;
                }
                else
                    DbHelpers.BinarySearch.BinaryStepDown(ref max, ref position, min);
            }
        }
    }
}
