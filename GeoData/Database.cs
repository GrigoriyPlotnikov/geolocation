using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoData.DbModel;
using System.Runtime.InteropServices;
using System.Buffers;

namespace GeoData
{
    public class Database
    {
        private readonly Header header;

        private readonly IpRange[] ips;

        private readonly Location[] locations;

        private readonly int[] indexes_sorted;


        public unsafe Database(string file)
        {
            using (var fs = System.IO.File.OpenRead("geobase.dat"))
            {
                {
                    Span<byte> headerBytes = stackalloc byte[sizeof(Header)];
                    fs.Read(headerBytes);
                    header = MemoryMarshal.AsRef<Header>(headerBytes);
                }

                ips = new IpRange[header.records];
                Span<byte> ipRangeBytes = stackalloc byte[sizeof(IpRange)];
                for (int i = 0; i < header.records; i++)
                {
                    fs.Read(ipRangeBytes);
                    ips[i] = MemoryMarshal.AsRef<IpRange>(ipRangeBytes);
                }

                locations = new Location[header.records];
                Span<byte> locBytes = stackalloc byte[sizeof(Location)];
                for (int i = 0; i < header.records; i++)
                {
                    fs.Read(locBytes);
                    locations[i] = MemoryMarshal.AsRef<Location>(locBytes);
                }

                indexes_sorted = new int[header.records];
                Span<byte> indexBytes = stackalloc byte[sizeof(int) * header.records];
                fs.Read(indexBytes);
                for (int i = 0; i < header.records; i++)
                {
                    indexes_sorted[i] = MemoryMarshal.AsRef<int>(indexBytes) / 96;
                    indexBytes = indexBytes.Slice(sizeof(int));
                }
            }
        }

        public Location GetLocationByIP(string ipStr)
        {
            return default;
        }

        /// <summary>
        /// checks consistensy of the database
        /// each ip range.location_index points at valid existing locations
        /// each city index points at proper city
        /// </summary>
        /// <param name="v"></param>
        public void SelfTest()
        {
            foreach (var range in ips)
            {
                var location = locations[range.location_index];
            }

            foreach (var index in indexes_sorted)
            {
                var location = locations[index];
            }
        }

        public string FirstDuplicate()
        {
            var q = new HashSet<string>();
            foreach (var loc in locations)
            {
                if (q.Contains(loc.City))
                    return loc.City;
                else
                    q.Add(loc.City);
            }
            return null;
        }

        public IEnumerable<Location> GetCityLocations(string city)
        {
            int max = header.records;
            int position = header.records / 2;
            int min = 0;

            //maxlength constrain iterations
            int maxlength = header.records;
            while (maxlength > 0)
            {
                maxlength--;

                var index = indexes_sorted[position];
                if (String.Compare(locations[index].City, city) == 0)
                {
                    //we get to a point that fits, but we need to move to top of the list of fitting entries.
                    if (index > 0 && String.Compare(locations[indexes_sorted[position-1]].City, city) == 0)
                    {
                        max = position;
                        //todo: consder smaller step
                        position -= (position - min) / 2;
                        continue;
                    }
                    //we get to top of list lets return all the cities
                    while (position < header.records && String.Compare(locations[indexes_sorted[position]].City, city) == 0)
                    {
                        yield return locations[indexes_sorted[position]];
                        position++;
                    }
                    //all cities are returned stop the enumeration
                    break;
                }
                else if (String.Compare(locations[index].City, city) > 0) 
                {
                    max = position;
                    position -= (position - min) / 2;
                    continue;
                }
                else
                {
                    min = position;
                    position += (max - position) / 2;
                }
            }
        }
    }
}
