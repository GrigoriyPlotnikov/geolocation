using System.Net;

namespace GeoData.DbHelpers
{
    public static class IpAddressHelper
    {
        public static void IPv4(string ipStr)
        {
            var ip = IPAddress.Parse(ipStr);
            
        }
    }
}
