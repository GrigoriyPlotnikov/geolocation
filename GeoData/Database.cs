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
            var bytes = System.IO.File.ReadAllBytes("geobase.dat");
            
            Span<byte> headerBytes = bytes.AsSpan(0, sizeof(Header));
            header = MemoryMarshal.AsRef<Header>(headerBytes);

            Span<byte> ipRangeBytes = bytes.AsSpan(
                start: (int) header.offset_ranges, 
                length: (int) (header.offset_locations - header.offset_ranges)
                );
            ips = MemoryMarshal.Cast<byte, IpRange>(ipRangeBytes).ToArray();

            //locations = new Location[header.records];
            Span<byte> locBytes = bytes.AsSpan(
                start: (int)header.offset_locations,
                length: (int)(header.offset_cities - header.offset_locations)
                );
            locations = MemoryMarshal.Cast<byte, Location>(locBytes).ToArray();

            Span<byte> indexBytes = bytes.AsSpan((int)header.offset_cities);
            indexes_sorted = MemoryMarshal.Cast<byte, int>(indexBytes).ToArray();

            //in file stored offsets , not indexes, lets fix that
            for (int i = 0; i < indexes_sorted.Length; i++)
                indexes_sorted[i] /= sizeof(Location);
        }

        public string Name { get { return header.Name; } }

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

        private int Compare(int position, string needle)
        {
            return String.Compare(locations[indexes_sorted[position]].City, needle);
        }

        public IEnumerable<Location> GetCityLocations(string city)
        {
            foreach (var position in DbHelpers.BinarySearch.SearchMany<string>(city, Compare, indexes_sorted.Length))
                yield return locations[indexes_sorted[position]];
        }
    }
}
