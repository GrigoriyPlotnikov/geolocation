using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.Contracts
{
    public interface IGeoIp
    {
        IEnumerable<ILocation> GetCityLocations(string city);

        ILocation GetLocationByIP(string ipStr);
    }
}
