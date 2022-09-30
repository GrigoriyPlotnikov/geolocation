using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoData.DbModel;
using System.Runtime.InteropServices;

namespace GeoData
{
    public class Database
    {
        private readonly Header header;

        private readonly IpRange[] ips;

        private readonly Location[] locations;

        private readonly int[] indexes_sorted;


        public Database(string file)
        {
            using (var fs = File.OpenRead("geobase.dat"))
            {
                //read header
                Span<byte> headerBytes = stackalloc byte[60];
                fs.Read(headerBytes);
                int pointer = 0;
                header = new Header
                {
                    version = ReadInteger(ref pointer, headerBytes),
                    name = ReadString(ref pointer, headerBytes, 32),
                    timestamp = ReadUnsignedLong(ref pointer, headerBytes),
                    records = ReadInteger(ref pointer, headerBytes),
                    offset_ranges = ReadUnsignedInteger(ref pointer, headerBytes),
                    offset_cities = ReadUnsignedInteger(ref pointer, headerBytes),
                    offset_locations = ReadUnsignedInteger(ref pointer, headerBytes),
                };

                ips = new IpRange[header.records];
                Span<byte> rangeBytes = stackalloc byte[12];
                for (int i = 0; i < header.records; i++)
                {
                    pointer = 0;
                    fs.Read(rangeBytes);
                    ips[i] = new IpRange()
                    {
                        ip_from = ReadUnsignedInteger(ref pointer, rangeBytes),
                        ip_to = ReadUnsignedInteger(ref pointer, rangeBytes),
                        location_index = ReadUnsignedInteger(ref pointer, rangeBytes)
                    };
                }

                locations = new Location[header.records];
                Span<byte> locationBytes = stackalloc byte[96];
                for (int i = 0; i < header.records; i++)
                {
                    pointer = 0;
                    fs.Read(locationBytes);
                    locations[i] = new Location
                    {
                        country = ReadString(ref pointer, locationBytes, 8),
                        region = ReadString(ref pointer, locationBytes, 12),
                        postal = ReadString(ref pointer, locationBytes, 12),
                        city = ReadString(ref pointer, locationBytes, 24),
                        organization = ReadString(ref pointer, locationBytes, 32),
                        latitude = ReadFloat(ref pointer, locationBytes),
                        longitude = ReadFloat(ref pointer, locationBytes),
                    };
                }

                indexes_sorted = new int[header.records];
                Span<byte> indexBytes = stackalloc byte[sizeof(int) * header.records];
                fs.Read(indexBytes);
                pointer = 0;
                for (int i = 0; i < header.records; i++)
                {
                    indexes_sorted[i] = ReadInteger(ref pointer, indexBytes) / 96;
                }

            }
        }

        public Location GetLocationByIP(string ipStr)
        {
            return null;
        }

        private string ReadString(ref int pointer, Span<byte> span, int length)
        {
            var res = System.Text.Encoding.ASCII.GetString(span.Slice(pointer, length));
            pointer += length;
            return res;
        }

        private int ReadInteger(ref int pointer, Span<byte> span)
        {
            var res = MemoryMarshal.Read<int>(span.Slice(pointer, sizeof(int)));
            pointer += sizeof(int);
            return res;
        }

        private float ReadFloat(ref int pointer, Span<byte> span)
        {
            var res = MemoryMarshal.Read<float>(span.Slice(pointer, sizeof(float)));
            pointer += sizeof(float);
            return res;
        }

        private uint ReadUnsignedInteger(ref int pointer, Span<byte> span)
        {
            var res = MemoryMarshal.Read<uint>(span.Slice(pointer, sizeof(uint)));
            pointer += sizeof(uint);
            return res;
        }

        private ulong ReadUnsignedLong(ref int pointer, Span<byte> span)
        {
            var res = MemoryMarshal.Read<ulong>(span.Slice(pointer, sizeof(ulong)));
            pointer += sizeof(ulong);
            return res;
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
                if (q.Contains(loc.city))
                    return loc.city;
                else
                    q.Add(loc.city);
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
                if (String.Compare(locations[index].city, city) == 0)
                {
                    //we get to a point that fits, but we need to move to top of the list of fitting entries.
                    if (index > 0 && String.Compare(locations[indexes_sorted[position-1]].city, city) == 0)
                    {
                        max = position;
                        //todo: consder smaller step
                        position -= (position - min) / 2;
                        continue;
                    }
                    //we get to top of list lets return all the cities
                    while (position < header.records && String.Compare(locations[indexes_sorted[position]].city, city) == 0)
                    {
                        yield return locations[indexes_sorted[position]];
                        position++;
                    }
                    //all cities are returned stop the enumeration
                    break;
                }
                else if (String.Compare(locations[index].city, city) > 0) 
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
