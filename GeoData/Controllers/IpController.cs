using GeoData.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoData.Controllers
{
    [Route("ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private IGeoIp db;

        public IpController(IGeoIp db)
        {
            this.db = db;
        }
        // GET: ip/location
        [Route("location")]
        [HttpGet]
        public ILocation GetLocation(string ip)
        {
            return db.GetLocationByIP(ip);
        }
    }
}
