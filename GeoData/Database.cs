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
        private readonly byte[] bytes;

        private readonly Header header;

        private ReadOnlySpan<IpRange> ips
        {
            get
            {
                Span<byte> ipRangeBytes = bytes.AsSpan(
                    start: (int)header.offset_ranges,
                    length: (int)(header.offset_locations - header.offset_ranges)
                    );
                return MemoryMarshal.Cast<byte, IpRange>(ipRangeBytes).ToArray();
            }
        }

        private ReadOnlySpan<Location> locations
        {
            get
            {
                Span<byte> locBytes = bytes.AsSpan(
                    start: (int)header.offset_locations,
                    length: (int)(header.offset_cities - header.offset_locations)
                    );
                return MemoryMarshal.Cast<byte, Location>(locBytes);
            }
        }

        private ReadOnlySpan<int> indexes_sorted
        {
            get
            {
                Span<byte> indexBytes = bytes.AsSpan((int)header.offset_cities);
                return MemoryMarshal.Cast<byte, int>(indexBytes);
            }
        }


        public unsafe Database(string file)
        {
            bytes = System.IO.File.ReadAllBytes("geobase.dat");

            Span<byte> headerBytes = bytes.AsSpan(0, sizeof(Header));
            header = MemoryMarshal.AsRef<Header>(headerBytes);

            Span<byte> indexBytes = bytes.AsSpan((int)header.offset_cities);
            fixed (int* p = MemoryMarshal.Cast<byte, int>(indexBytes))
            {
                for (int i = 0; i < indexes_sorted.Length; i++)
                    p[i] = p[i] / sizeof(Location);
            }
        }

        public string Name { get { return header.Name; } }

        public Location GetLocationByIP(string ipStr)
        {
            return default;
        }

        public void ConsistencyCheck()
        {
            foreach (var range in ips)
            {
                var location = locations[(int)range.location_index];
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
            return locations[indexes_sorted[position]].CompareCity(needle);
        }

        public IEnumerable<Location> GetCityLocations(string city)
        {
            foreach (var position in DbHelpers.BinarySearch.SearchMany<string>(city, Compare, indexes_sorted.Length))
                yield return locations[indexes_sorted[position]];
        }
    }
}
