using System;
using System.Runtime.InteropServices;

namespace GeoData.DbModel
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct IpRange
    {
        [FieldOffset(0)]
        public fixed byte ip_from[4];           // начало диапазона IP адресов
        [FieldOffset(4)]
        public fixed byte ip_to[4];             // конец диапазона IP адресов
        [FieldOffset(8)]
        public uint location_index;    // индекс записи о местоположении

        internal int CompareAddress(byte[] needle)
        {
            if (needle == null)
                return -1;

            int len = 4;
            //ipV4 only
            if (needle.Length != len)
                return 1;

            return 0;

            //fixed (byte* fromPtr = ip_from)
            //fixed (byte* toPtr = ip_to)
            //{
            //    for (int i = 0; i < len; i++)
            //    {
            //        if (i)
            //    }
            //}
        }
    }
}
