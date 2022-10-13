using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.Contracts
{
    public interface IGeoIp
    {
        IEnumerable<ILocation> GetCityLocations(string city);

        /// <exception cref="InvalidOperationException">is thrown if the ip does not parsed</exception>
        /// <exception cref="NotFoundException">is thrown if the ip does not exists in db</exception>
        ILocation GetLocationByIP(string ipStr);
    }
}
