using System.Net;

namespace GeoData.Db.Helpers
{
    public static class IpAddress
    {
        public static uint? GetAddress(string ipStr)
        {
            var ipBytes = new byte[4];
            int byteIndex = 0;
            //ipV4 only
            for (int i = 0; i < ipStr.Length; i++)
            {
                if (char.IsDigit(ipStr[i]))
                {
                    ipBytes[byteIndex] = (byte)(ipBytes[byteIndex] * 10 + (int)ipStr[i] - 0x30);
                }
                else if (ipStr[i].Equals('.'))
                {
                    byteIndex++;
                    if (byteIndex == 4)
                        return null;
                }
                else
                    return null;
            }

            return ((uint)(ipBytes[0] << 24)) |
               ((uint)(ipBytes[1] << 16)) |
               ((uint)(ipBytes[2] << 8)) |
               ((uint)(ipBytes[3]));
        }
    }
}
