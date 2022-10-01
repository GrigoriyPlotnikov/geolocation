using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoData.DbHelpers;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class Helpers
    {

        [TestMethod]
        public void BinaryStepUp()
        {
            int min = 0;
            int position = 10;
            int max = 20;
            BinarySearch.BinaryStepUp(max, ref position, ref min);
            Assert.AreEqual(15, position);
            Assert.AreEqual(10, min);

            BinarySearch.BinaryStepUp(max, ref position, ref min);
            Assert.AreEqual(17, position);
            Assert.AreEqual(15, min);
        }

        [TestMethod]
        public void BinaryStepDown()
        {
            int min = 0;
            int position = 10;
            int max = 20;
            BinarySearch.BinaryStepDown(ref max, ref position, min);
            Assert.AreEqual(5, position);
            Assert.AreEqual(10, max);

            BinarySearch.BinaryStepDown(ref max, ref position, min);
            Assert.AreEqual(3, position);
            Assert.AreEqual(5, max);
        }

        [TestMethod]
        public void BinarySearchMany()
        {
            int[] data = { 1, 2, 3, 3, 4, 5 };
            var res = BinarySearch.SearchMany(3, (index, needle) => data[index].CompareTo(needle), data.Length);
            Assert.AreEqual(2, res.Count());
        }
    }
}
