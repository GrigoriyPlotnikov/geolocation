using GeoData;
using GeoData.Contracts;
using GeoData.Db.Model;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class ControllerTests : IAsyncLifetime
    {
        private readonly Mock<IGeoIp> _dbMock = new();
        private HttpClient _httpClient;

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var hostBuilder = Program.CreateHostBuilder(new string[0])
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder.UseTestServer();
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(_dbMock.Object);
                });

            var host = await hostBuilder.StartAsync();
            _httpClient = host.GetTestClient();
        }

        [Fact]
        public async Task IpController_HappyPath()
        {
            var ip = "123.234.123.234";
            var loc = new LocationDTO()
            {
                City = "Limassol",
                Country = "Cyprus",
                Organization = "Metaquotes"
            };

            _dbMock
                .Setup(dbm => dbm.GetLocationByIP(ip))
                .Returns(loc);

            var response = await _httpClient.GetAsync($"ip/location/?ip={ip}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedJson = await response.Content.ReadAsStringAsync();
            var returnedLocation = JsonConvert.DeserializeObject<LocationDTO>(returnedJson);
            Assert.Equal(loc.City, returnedLocation.City);
        }

        [Fact]
        public async Task IpController_NotFoundException_404()
        {
            var ip = "8.8.8.8";
            var givenException = new NotFoundException("foo");
            var resultingStatusCode = HttpStatusCode.NotFound;

            _dbMock
                .Setup(dbm => dbm.GetLocationByIP(ip))
                .Throws(givenException);

            var response = await _httpClient.GetAsync($"ip/location/?ip={ip}");
            Assert.Equal(resultingStatusCode, response.StatusCode);
        }


        [Fact]
        public async Task CityController_HappyPath()
        {
            var city = "Limassol";
            var locs = new[] {
                new LocationDTO()
                {
                    City = "Limassol",
                    Country = "Cyprus",
                    Organization = "Metaquotes"
                }
            };

            _dbMock
                .Setup(dbm => dbm.GetCityLocations(city))
                .Returns(locs.Select(l => (ILocation)l));

            var response = await _httpClient.GetAsync($"city/locations/?city={city}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedJson = await response.Content.ReadAsStringAsync();
            var returnedLocation = JsonConvert.DeserializeObject<LocationDTO[]>(returnedJson);
            Assert.Equal(locs.Length, returnedLocation.Length);
        }

        [Fact]
        public async Task CityController_NotFound()
        {
            var city = "Limassol";

            _dbMock
                .Setup(dbm => dbm.GetCityLocations(city))
                .Returns(Enumerable.Empty<ILocation>());

            var response = await _httpClient.GetAsync($"city/locations/?city={city}");
            var returnedJson = await response.Content.ReadAsStringAsync();
            var returnedLocation = JsonConvert.DeserializeObject<Location[]>(returnedJson);
            Assert.True(returnedLocation.Length == 0);
        }

        private class LocationDTO : ILocation
        {
            public string Country { get; set; }
            public string Region { get; set; }
            public string City { get; set; }
            public string Postal { get; set; }
            public string Organization { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }
    }
}
