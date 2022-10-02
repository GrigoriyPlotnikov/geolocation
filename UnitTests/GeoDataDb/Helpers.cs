using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoData.Db.Helpers;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UnitTests.GeoDataDb
{
    [TestClass]
    public class HelpersTests
    {

        [TestMethod]
        public void BinarySearchMany()
        {
            int[] data = { 1, 2, 3, 3, 4, 5 };

            int position;
            
            //many. left index of 3 is 2
            Assert.AreEqual(2, BinarySearch.SearchLeftmost<int>(3, (index, needle) => needle.CompareTo(data[index]), data.Length));

            //one
            Assert.AreEqual(1, BinarySearch.SearchLeftmost<int>(2, (index, needle) => needle.CompareTo(data[index]), data.Length));

            //range edges
            Assert.AreEqual(0, BinarySearch.SearchLeftmost<int>(1, (index, needle) => needle.CompareTo(data[index]), data.Length));
            Assert.AreEqual(5, BinarySearch.SearchLeftmost<int>(5, (index, needle) => needle.CompareTo(data[index]), data.Length));
            
            //nothin
            position = BinarySearch.SearchLeftmost<int>(8, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.AreEqual(6, position);


            IEnumerable<int> res;
            
            //many
            res = BinarySearch.SearchMany(3, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.AreEqual(2, res.Count());

            //one
            res = BinarySearch.SearchMany(2, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.AreEqual(1, res.Count());

            //nothing
            res = BinarySearch.SearchMany(8, (index, needle) => needle.CompareTo(data[index]), data.Length);
            Assert.AreEqual(0, res.Count());
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

        [TestMethod]

        public void IpAddressStringToUint()
        {
            var ipStr = "123.234.123.234";
            var ipUint = IpAddress.GetAddress(ipStr);

            Assert.AreEqual(ipStr, string.Join(".", BitConverter.GetBytes(ipUint.Value).Select(b => b.ToString("G"))));
        }
    }
}
