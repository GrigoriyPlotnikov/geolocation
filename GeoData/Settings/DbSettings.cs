using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.Settings
{
    public class DbSettings
    {
        public string GeoIpPath { get; set; }

        public override string ToString()
        {
            var lines = new[]
            {
                $"\tGeoIpPath: {GeoIpPath}",
            };

            return string.Join(Environment.NewLine, lines);
        }
    }
}
