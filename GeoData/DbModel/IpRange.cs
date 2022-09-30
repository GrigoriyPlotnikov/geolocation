using System.Runtime.InteropServices;

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
    }
}
