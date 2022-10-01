using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoData.DbHelpers;
using System.Linq;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class Helpers
    {

        [TestMethod]
        public void BinarySearchMany()
        {
            int[] data = { 1, 2, 3, 3, 4, 5 };

            bool found = false;
            List<int> res = new List<int>();
            int position = BinarySearch.SearchLeftmost<int>(3, (index, needle) => needle.CompareTo(data[index]), data.Length);
            do
            {
                found = 3.CompareTo(data[position]) == 0;
                if (found)
                {
                    res.Add(data[position]);
                    position++;
                }
            }
            while (position < data.Length && found);
            Assert.AreEqual(2, res.Count());
        }

        [TestMethod]
        public void BinarySearchOne()
        {
            int[] data = { 1, 2, 3, 3, 4, 5 };
            var res = BinarySearch.Search(3, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.IsNotNull(res);

            res = BinarySearch.Search(1, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.IsNotNull(res);

            res = BinarySearch.Search(5, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.IsNotNull(res);

            res = BinarySearch.Search(8, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.IsNull(res);
        }
    }
}
