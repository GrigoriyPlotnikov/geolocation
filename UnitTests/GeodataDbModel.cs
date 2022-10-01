using GeoData.DbModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace UnitTests
{
    [TestClass]
    public class GeodataDbModel
    {
        [TestMethod]
        public void CompareCity()
        {
            //Span<byte> bytes = new byte[] { 99, 111, 117, 95, 85, 74, 79, 0, 114, 101, 103, 95, 85, 0, 0, 0, 0, 0, 0, 0, 112, 111, 115, 95, 53, 56, 50, 52, 50, 51, 0, 0, 99, 105, 116, 95, 69, 108, 117, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 111, 114, 103, 95, 69, 98, 97, 32, 65, 98, 97, 99, 105, 114, 32, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 170, 130, 192, 194, 49, 25, 78, 194 }
            //    .AsSpan<byte>();

            var location = new Location();
            location.City = "cit_Kazan";

            Assert.AreEqual(0, location.CompareCity("cit_Kazan"));

            Assert.IsTrue(location.CompareCity("cit_Nikosia") > 0);
        }

        [TestMethod]
        public void CompareIpRange()
        {
            var range = new IpRange();
            range.ip_from = 1;
            range.ip_to = 3;

            Assert.AreEqual(0, range.CompareAddress(1));
            Assert.AreEqual(0, range.CompareAddress(2));
            Assert.AreEqual(0, range.CompareAddress(3));

            Assert.AreEqual(-1, range.CompareAddress(4));

            Assert.AreEqual(1, range.CompareAddress(0));

        }
    }
}
