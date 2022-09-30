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
            var fileBytes = System.IO.File.ReadAllBytes("geobase.dat");
            int pointer = 0;
            //read header
            header = new Header
            {
                version = ReadInteger(ref pointer, fileBytes),
                name = ReadString(ref pointer, fileBytes, 32),
                timestamp = ReadUnsignedLong(ref pointer, fileBytes),
                records = ReadInteger(ref pointer, fileBytes),
                offset_ranges = ReadUnsignedInteger(ref pointer, fileBytes),
                offset_cities = ReadUnsignedInteger(ref pointer, fileBytes),
                offset_locations = ReadUnsignedInteger(ref pointer, fileBytes),
            };

            ips = new IpRange[header.records];
            ReadIps(ref pointer, fileBytes);

            locations = new Location[header.records];
            ReadLocations(ref pointer, fileBytes);

            indexes_sorted = new int[header.records];
            for (int i = 0; i < header.records; i++)
                indexes_sorted[i] = ReadInteger(ref pointer, fileBytes) / 96; // size of location structure

        }

        private int ReadInteger(ref int pointer, byte[] fileBytes)
        {
            var res = BitConverter.ToInt32(fileBytes, pointer);
            pointer += sizeof(int);
            return res;
        }

        private float ReadFloat(ref int pointer, byte[] fileBytes)
        {
            var res = BitConverter.ToSingle(fileBytes, pointer);
            pointer += sizeof(float);
            return res;
        }

        private uint ReadUnsignedInteger(ref int pointer, byte[] fileBytes)
        {
            var res = BitConverter.ToUInt32(fileBytes, pointer);
            pointer += sizeof(uint);
            return res;
        }

        private ulong ReadUnsignedLong(ref int pointer, byte[] fileBytes)
        {
            var res = BitConverter.ToUInt64(fileBytes, pointer);
            pointer += sizeof(ulong);
            return res;
        }

        private string ReadString(ref int pointer, byte[] fileBytes, int length)
        {
            var res = System.Text.Encoding.ASCII.GetString(fileBytes, pointer, length);
            pointer += length;
            return res;
        }


        private void ReadLocations(ref int pointer, byte[] fileBytes)
        {
            for (int i = 0; i < header.records; i++)
            {
                locations[i] = new Location
                {
                    country = ReadString(ref pointer, fileBytes, 8),
                    region = ReadString(ref pointer, fileBytes, 12),
                    postal = ReadString(ref pointer, fileBytes, 12),
                    city = ReadString(ref pointer, fileBytes, 24),
                    organization = ReadString(ref pointer, fileBytes, 32),
                    latitude = ReadFloat(ref pointer, fileBytes),
                    longitude = ReadFloat(ref pointer, fileBytes),
                };
            }
        }

        private void ReadIps(ref int pointer, byte[] fileBytes)
        {
            for (int i = 0; i < header.records; i++)
            {
                ips[i] = new IpRange()
                {
                    ip_from = ReadUnsignedInteger(ref pointer, fileBytes),
                    ip_to = ReadUnsignedInteger(ref pointer, fileBytes),
                    location_index = ReadUnsignedInteger(ref pointer, fileBytes)
                };
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
