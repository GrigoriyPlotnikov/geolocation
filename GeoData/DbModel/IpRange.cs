using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeoData.DbModel
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
                return 1;
            if (ip_to < needle)
                return -1;

            return 0;
        }
    }
}
