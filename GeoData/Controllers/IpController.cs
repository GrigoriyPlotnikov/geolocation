using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoData.Controllers
{
    [Route("ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private Db.GeoIp db;

        public IpController(Db.GeoIp db)
        {
            this.db = db;
        }
        // GET: ip/location
        [Route("location")]
        [HttpGet]
        public Db.Model.Location? GetLocation(string ip)
        {
            return db.GetLocationByIP(ip);
        }
    }
}
