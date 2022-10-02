using System.Runtime.InteropServices;
using System.Text;

namespace GeoData.Db.Model
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Header
    {
        [FieldOffset(0)]
        public int version;           // версия база данных
        [FieldOffset(4)]
        public fixed sbyte name[32];          // название/префикс для базы данных
        [FieldOffset(36)]
        public ulong timestamp;         // время создания базы данных
        [FieldOffset(44)]
        public int records;           // общее количество записей
        [FieldOffset(48)]
        public uint offset_ranges;     // смещение относительно начала файла до начала списка записей с геоинформацией
        [FieldOffset(52)]
        public uint offset_cities;     // смещение относительно начала файла до начала индекса с сортировкой по названию городов
        [FieldOffset(56)]
        public uint offset_locations;  // смещение относительно начала файла до начала списка записей о местоположении

        public string Name
        {
            get
            {
                fixed (sbyte* namePtr = name)
                {
                    int len = 32;
                    for (int i = 0; i < len; i++)
                        if (namePtr[i] == 0x0)
                            len = i;

                    return new string(namePtr, 0, len, Encoding.ASCII);
                }
            }
        }
    }
}
