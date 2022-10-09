using GeoData.Db.Helpers;
using GeoData.Db.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.GeoDataDb
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void CompareCity()
        {
            var location = new Location
            {
                City = "cit_Kazan"
            };

            Assert.AreEqual(0, location.CompareCity("cit_Kazan"));

            Assert.IsTrue(location.CompareCity("cit_Nikosia") > 0);
        }

        [TestMethod]
        public void CompareIpRange()
        {
            var range = new IpRange
            {
                From = "118.83.161.94",
                To = "118.83.207.177"
            };

            Assert.AreEqual(0, range.CompareAddress(IpAddress.GetAddress("118.83.161.94").Value));
            Assert.AreEqual(0, range.CompareAddress(IpAddress.GetAddress("118.83.161.95").Value));
            Assert.AreEqual(0, range.CompareAddress(IpAddress.GetAddress("118.83.161.123").Value));
            Assert.AreEqual(0, range.CompareAddress(IpAddress.GetAddress("118.83.161.177").Value));

            Assert.AreEqual(-1, range.CompareAddress(IpAddress.GetAddress("118.83.161.93").Value));

            Assert.AreEqual(1, range.CompareAddress(IpAddress.GetAddress("118.83.207.178").Value));

        }
    }
}
