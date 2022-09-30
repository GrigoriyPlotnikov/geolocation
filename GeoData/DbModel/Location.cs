using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoData.DbModel
{
    public class Location
    {
        public string country;        // название страны (случайная строка с префиксом "cou_")
        public string region;        // название области (случайная строка с префиксом "reg_")
        public string postal;        // почтовый индекс (случайная строка с префиксом "pos_")
        public string city;          // название города (случайная строка с префиксом "cit_")
        public string organization;  // название организации (случайная строка с префиксом "org_")
        public float latitude;          // широта
        public float longitude;         // долгота
    }
}
