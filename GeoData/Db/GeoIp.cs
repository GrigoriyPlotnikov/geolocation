using System;
using System.Collections.Generic;
using GeoData.Db.Model;
using System.Runtime.InteropServices;
using GeoData.Db.Helpers;
using Microsoft.Extensions.Options;
using GeoData.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GeoData.Db
{
    public class GeoIp : IGeoIp
    {
        private readonly byte[] _bytes;

        private readonly Header header;

        private ReadOnlySpan<IpRange> IpAddresses
        {
            get
            {
                Span<byte> ipRangeBytes = _bytes.AsSpan(
                    start: (int)header.offset_ranges,
                    length: (int)(header.offset_locations - header.offset_ranges)
                    );
                return MemoryMarshal.Cast<byte, IpRange>(ipRangeBytes);
            }
        }

        private ReadOnlySpan<Location> Locations
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

        private ReadOnlySpan<int> IndexesCity
        {
            get
            {
                Span<byte> indexBytes = _bytes.AsSpan((int)header.offset_cities);
                return MemoryMarshal.Cast<byte, int>(indexBytes);
            }
        }

        public unsafe GeoIp(IOptions<Settings.DbSettings> settings, ILogger<IGeoIp> logger)
        {
            logger.LogInformation(settings.Value.ToString());
            _bytes = System.IO.File.ReadAllBytes(settings.Value.GeoIpPath);

            Span<byte> headerBytes = _bytes.AsSpan(0, sizeof(Header));
            header = MemoryMarshal.AsRef<Header>(headerBytes);

            Span<byte> indexBytes = _bytes.AsSpan((int)header.offset_cities);
            fixed (int* p = MemoryMarshal.Cast<byte, int>(indexBytes))
            {
                for (int i = 0; i < IndexesCity.Length; i++)
                    p[i] = p[i] / sizeof(Location);
            }
        }

        public string Name { get { return header.Name; } }

        public Task<ILocation> GetLocationByIP(string ipStr)
        {
            var ipUint = IpAddress.GetAddress(ipStr);

            if (ipUint == null)
                throw new InvalidOperationException(ipStr);

            return Task.Run<ILocation>(() =>
            {
                var pos = BinarySearch.Search<uint>(ipUint.Value, CompareIpAddess, IpAddresses.Length);

                if (pos == null)
                    throw new NotFoundException(ipStr);

                return Locations[(int)IpAddresses[pos.Value].location_index];
            });
        }

        private int CompareIpAddess(int position, uint needle)
        {
            return IpAddresses[position].CompareAddress(needle);
        }

        public void ConsistencyCheck()
        {
            foreach (var range in IpAddresses)
            {
                var index = (int)range.location_index;
                if (index > 0 && index < Locations.Length) { }
            }

            foreach (var index in IndexesCity)
            {
                if (index > 0 && index < Locations.Length) { }
            }
        }

        private int CompareCityNames(int position, string needle)
        {
            return Locations[IndexesCity[position]].CompareCity(needle);
        }

        public IEnumerable<ILocation> GetCityLocations(string city)
        {
            foreach (var position in BinarySearch.SearchMany<string>(city, CompareCityNames, IndexesCity.Length))
                yield return Locations[IndexesCity[position]];
        }
    }
}
