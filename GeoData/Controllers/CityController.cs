using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoData.Controllers
{
    [Route("city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private Db.GeoIp db;

        public CityController(Db.GeoIp db)
        {
            this.db = db;
        }

        // GET: city/locations
        [Route("locations")]
        [HttpGet]
        public IEnumerable<Db.Model.Location> GetLocation(string city)
        {
            return db.GetCityLocations(city);
        }
    }
}
