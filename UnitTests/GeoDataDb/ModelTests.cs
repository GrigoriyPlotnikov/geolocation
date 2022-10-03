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
                ip_from = 1,
                ip_to = 3
            };

            Assert.AreEqual(0, range.CompareAddress(1));
            Assert.AreEqual(0, range.CompareAddress(2));
            Assert.AreEqual(0, range.CompareAddress(3));

            Assert.AreEqual(-1, range.CompareAddress(4));

            Assert.AreEqual(1, range.CompareAddress(0));

        }
    }
}
