using System.Runtime.InteropServices;
using System.Text;

namespace GeoData.DbModel
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Location
    {
        [FieldOffset(0)]
        public fixed sbyte country[8];        // название страны (случайная строка с префиксом "cou_")
        [FieldOffset(8)]
        public fixed sbyte region[12];        // название области (случайная строка с префиксом "reg_")
        [FieldOffset(20)]
        public fixed sbyte postal[12];        // почтовый индекс (случайная строка с префиксом "pos_")
        [FieldOffset(32)]
        public fixed sbyte city[24];          // название города (случайная строка с префиксом "cit_")
        [FieldOffset(56)]
        public fixed sbyte organization[32];  // название организации (случайная строка с префиксом "org_")
        [FieldOffset(88)]
        public float latitude;          // широта
        [FieldOffset(92)]
        public float longitude;         // долгота

        public string City
        {
            get
            {
                fixed (sbyte* namePtr = city)
                {
                    int len = 24;
                    for (int i = 0; i < len; i++)
                        if (namePtr[i] == 0x0)
                            len = i;

                    return new string(namePtr, 0, len, Encoding.ASCII);
                }
            }
        }
    }
}
