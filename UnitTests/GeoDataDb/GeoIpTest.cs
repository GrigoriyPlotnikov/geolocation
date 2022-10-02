using GeoData.Contracts;
using GeoData.Settings;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using UnitTests.Mock;

namespace UnitTests.GeoDataDb
{
    [TestClass]
    public class GeoIpTests
    {
        private LoggerMock<IGeoIp> loggerMock;
        private Mock<IOptions<DbSettings>> databaseSettingsMock;

        GeoData.Db.GeoIp database;

        [TestInitialize]
        public void Initialize()
        {
            loggerMock = new LoggerMock<IGeoIp>();
            databaseSettingsMock = new Mock<IOptions<DbSettings>>();

            databaseSettingsMock
                .Setup(x => x.Value)
                .Returns(new DbSettings
                {
                    GeoIpPath = "geobase.dat",
                });

            database = new GeoData.Db.GeoIp(databaseSettingsMock.Object, loggerMock);
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
