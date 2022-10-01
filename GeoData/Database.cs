﻿using System;
using System.Collections.Generic;
using GeoData.DbModel;
using System.Runtime.InteropServices;
using GeoData.DbHelpers;

namespace GeoData
{
    public class Database
    {
        private readonly byte[] _bytes;

        private readonly Header header;

        private ReadOnlySpan<IpRange> ips
        {
            get
            {
                Span<byte> ipRangeBytes = _bytes.AsSpan(
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
                Span<byte> locBytes = _bytes.AsSpan(
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
                Span<byte> indexBytes = _bytes.AsSpan((int)header.offset_cities);
                return MemoryMarshal.Cast<byte, int>(indexBytes);
            }
        }

        public unsafe Database(string file)
        {
            _bytes = System.IO.File.ReadAllBytes("geobase.dat");

            Span<byte> headerBytes = _bytes.AsSpan(0, sizeof(Header));
            header = MemoryMarshal.AsRef<Header>(headerBytes);

            Span<byte> indexBytes = _bytes.AsSpan((int)header.offset_cities);
            fixed (int* p = MemoryMarshal.Cast<byte, int>(indexBytes))
            {
                for (int i = 0; i < indexes_sorted.Length; i++)
                    p[i] = p[i] / sizeof(Location);
            }
        }

        public string Name { get { return header.Name; } }

        public Location? GetLocationByIP(string ipStr)
        {
            var ipUint = IpAddress.GetAddress(ipStr);

            if (ipUint == null)
                return null;

            var pos = BinarySearch.Search<uint>(ipUint.Value, CompareIpAddess, ips.Length);

            if (pos == null)
                return null;

            return locations[(int)ips[pos.Value].location_index];
        }

        private int CompareIpAddess(int position, uint needle)
        {
            return ips[position].CompareAddress(needle);
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

        private int CompareCityNames(int position, string needle)
        {
            return locations[indexes_sorted[position]].CompareCity(needle);
        }

        public IEnumerable<Location> GetCityLocations(string city)
        {
            foreach (var position in DbHelpers.BinarySearch.SearchMany<string>(city, CompareCityNames, indexes_sorted.Length))
                yield return locations[indexes_sorted[position]];
        }
    }
}
