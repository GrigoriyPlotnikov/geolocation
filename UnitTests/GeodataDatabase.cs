using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class GeodataDatabase
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
            Assert.AreEqual("Geo.IP", database.Name);

            database.ConsistencyCheck();
        }

        [TestMethod]
        public void GetLocationsSuccess()
        {
            var locs = database.GetCityLocations("cit_Uwol Z Hyt Xavi");
            Assert.IsTrue(locs.Any());
        }

        [TestMethod]
        public void GetSeveralLocations()
        {
            var locs = database.GetCityLocations("cit_Uqoced");
            Assert.IsTrue(locs.Count() > 1);
        }

        [TestMethod]
        public void MissingLocation()
        {
            var locs = database.GetCityLocations("noting");
            Assert.IsFalse(locs.Any());

            //the specific sample from docs. worth checking but empty set
            locs = database.GetCityLocations("cit_Gbqw4");
            Assert.IsFalse(locs.Any());
        }

        [TestMethod]
        public void GetLocationByIpSuccess()
        {
            //empty test ?
            Assert.IsNull(database.GetLocationByIP("123.234.123.234"));

            //
            Assert.IsNotNull(database.GetLocationByIP("116.226.107.115"));
        }
    }
}
