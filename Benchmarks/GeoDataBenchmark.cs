using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    ///<summary>
    ///Added here as a reference point to check the requirement
    ///Время загрузки базы в память не должно превышать время, 
    ///затраченное на чтение данных с диска + 5 мс на парсинг данных, 
    ///создание индексов и прочие накладные расходы.
    /// </summary>
//|             Method |          Mean |        Error |        StdDev |   Allocated |
//|------------------- |--------------:|-------------:|--------------:|------------:|
//| ReadFileByteByByte | 155,190.13 us | 5,098.106 us | 15,031.880 us |     4.45 KB |
//|      ReadFileBytes |   6,483.61 us |   157.987 us |    465.828 us |    10938 KB |
//|       ReadFileText |  93,778.67 us | 2,460.125 us |  6,978.965 us | 43567.99 KB |
    [MemoryDiagnoser(false)]
    public class GeoDataBenchmark
    {
        public void ReadFileByteByByte()
        {
            using (var fileStream = System.IO.File.OpenRead("geobase.dat"))
            {
                while (fileStream.ReadByte() >= 0)
                    ;
            }
        }

        public void ReadFileBytes()
        {
            var q = System.IO.File.ReadAllBytes("geobase.dat");
        }

        public void ReadFileText()
        {
            var q = System.IO.File.ReadAllText("geobase.dat");
        }

        [Benchmark]
        public void ReadDatabase()
        {
            var q = new GeoData.Database("geobase.dat");
        }
    }
}
