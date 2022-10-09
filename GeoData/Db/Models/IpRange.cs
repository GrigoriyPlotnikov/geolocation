using GeoData.Db.Helpers;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GeoData.Db.Model
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct IpRange
    {
        [FieldOffset(0)]
        public uint ip_from;           // начало диапазона IP адресов
        [FieldOffset(4)]
        public uint ip_to;             // конец диапазона IP адресов
        [FieldOffset(8)]
        public uint location_index;    // индекс записи о местоположении

        public int CompareAddress(uint needle)
        {
            //1 //2 //3 //4
            if (ip_from > needle)
                return -1;
            if (ip_to < needle)
                return 1;

            return 0;
        }

        public override string ToString()
        {
            return $"Loc:{location_index} IPS:{From} - {To}";
        }

        public string From
        {
            get { return string.Join(".", BitConverter.GetBytes(ip_from).Reverse()); }
            set { ip_from = IpAddress.GetAddress(value).Value; }
        }

        public string To
        {
            get { return string.Join(".", BitConverter.GetBytes(ip_to).Reverse()); }
            set { ip_to = IpAddress.GetAddress(value).Value; }
        }
    }
}
