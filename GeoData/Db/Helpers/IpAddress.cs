﻿using System.Net;

namespace GeoData.Db.Helpers
{
    public static class IpAddress
    {
        public static uint? GetAddress(string ipStr)
        {
            var address = IPAddress.Parse(ipStr);
            var ipBytes = address.GetAddressBytes();

            //ipV4 only
            if (ipBytes.Length != 4)
                return null;

            return ((uint)(ipBytes[0] << 24)) |
               ((uint)(ipBytes[1] << 16)) |
               ((uint)(ipBytes[2] << 8)) |
               ((uint)(ipBytes[3]));
        }
    }
}
