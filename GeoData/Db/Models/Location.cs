using GeoData.Contracts;
using System.Runtime.InteropServices;
using System.Text;

namespace GeoData.Db.Model
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Location : ILocation
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
                    return DisplayAny(namePtr, 24);
                }
            }

            set
            {
                fixed (sbyte* namePtr = city)
                {
                    SetAny(namePtr, 24, value);
                }
            }
        }

        private static void SetAny(sbyte* ptr, int len, string value)
        {
            for (int i = 0; i < len; i++)
            {
                if (value.Length > i)
                    ptr[i] = (sbyte)value[i];
                else
                    ptr[i] = 0x0;
            }
        }

        private static string DisplayAny(sbyte* ptr, int len)
        {
            for (int i = 0; i < len; i++)
                if (ptr[i] == 0x0)
                    len = i;

            return new string(ptr, 0, len, Encoding.ASCII);
        }

        public int CompareCity(string needle)
        {
            if (needle == null)
                return -1;

            fixed (sbyte* namePtr = city)
            {
                int len = 24;
                for (int i = 0; i < len; i++)
                {
                    char current = (char)namePtr[i];
                    if (current == 0x0) //the line is over
                    {
                        //both lines are over
                        if (needle.Length <= i || needle[i] == 0x0)
                            return 0;
                        else
                            return 1; //the city name is shorter after accounting zero bytes
                    }

                    var res = needle[i].CompareTo(current);
                    if (res != 0)
                        return res;
                }
            }

            return 0;
        }

        public string Country
        {
            get
            {
                fixed (sbyte* namePtr = country)
                {
                    return DisplayAny(namePtr, 8);
                }
            }

            set
            {
                fixed (sbyte* namePtr = country)
                {
                    SetAny(namePtr, 8, value);
                }
            }
        }

        public string Postal
        {
            get
            {
                fixed (sbyte* namePtr = postal)
                {
                    return DisplayAny(namePtr, 12);
                }
            }

            set
            {
                fixed (sbyte* namePtr = postal)
                {
                    SetAny(namePtr, 12, value);
                }
            }
        }

        public string Region
        {
            get
            {
                fixed (sbyte* namePtr = region)
                {
                    return DisplayAny(namePtr, 12);
                }
            }

            set
            {
                fixed (sbyte* namePtr = region)
                {
                    SetAny(namePtr, 12, value);
                }
            }
        }

        public string Organization
        {
            get
            {
                fixed (sbyte* namePtr = organization)
                {
                    return DisplayAny(namePtr, 32);
                }
            }

            set
            {
                fixed (sbyte* namePtr = organization)
                {
                    SetAny(namePtr, 32, value);
                }
            }
        }

        public float Latitude { get { return latitude; } set { latitude = value; } }

        public float Longitude { get { return longitude; } set { longitude = value; } }
    }
}
