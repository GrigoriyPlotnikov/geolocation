using GeoData.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoData.Controllers
{
    [Route("city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private IGeoIp db;

        public CityController(IGeoIp db)
        {
            this.db = db;
        }

        // GET: city/locations
        [Route("locations")]
        [HttpGet]
        public IEnumerable<ILocation> GetLocation(string city)
        {
            return db.GetCityLocations(city);
        }
    }
}
