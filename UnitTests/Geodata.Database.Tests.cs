using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class Geodata
    {
        GeoData.Database database;

        [TestInitialize]
        public void LoadDatabase()
        {
            database = new GeoData.Database("geobase.dat");
        }

        [TestMethod]
        public void SelfTest()
        {
            database.SelfTest();
            Assert.IsNotNull(database.FirstDuplicate());

            Assert.AreEqual("Geo.IP", database.Name);
        }

        [TestMethod]
        public void TestGetLocationsSuccess()
        {
            var locs = database.GetCityLocations("cit_Uwol Z Hyt Xavi");
            Assert.IsTrue(locs.Any());
        }

        [TestMethod]
        public void TestGetSeveralLocations()
        {
            var locs = database.GetCityLocations("cit_Uqoced");
            Assert.IsTrue(locs.Count() > 1);
        }

        [TestMethod]
        public void TestMissingLocation()
        {
            var locs = database.GetCityLocations("noting");
            Assert.IsFalse(locs.Any());
        }

        /// <summary>
        /// the specific sample from docs. worth checking but empty set
        /// </summary>
        [TestMethod]
        public void TestGetLocationsSuccessSpecific()
        {
            var locs = database.GetCityLocations("cit_Gbqw4");
            Assert.IsFalse(locs.Any());
        }

        public void TestGetLocationSuccess()
        {
            var location = database.GetLocationByIP("123.234.123.234");
            Assert.IsNotNull(location);
        }
    }
}
