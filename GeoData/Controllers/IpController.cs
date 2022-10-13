using GeoData.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoData.Controllers
{
    [Route("ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeoIp db;

        public IpController(IGeoIp db)
        {
            this.db = db;
        }
        // GET: ip/location
        [Route("location")]
        [HttpGet]
        public ActionResult<ILocation> GetLocation(string ip)
        {
            try
            {
                var loc = db.GetLocationByIP(ip);
                return Ok(loc);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
