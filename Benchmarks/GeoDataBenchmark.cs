using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    ///<summary>
    /// Время загрузки базы в память не должно превышать время, 
    /// затраченное на чтение данных с диска + 5 мс на парсинг данных, 
    /// создание индексов и прочие накладные расходы.
    /// </summary>
    ///|             Method |          Mean |        Error |        StdDev |   Allocated | Ratio | Ratio SD | Alloc Ratio |
    ///|------------------- |--------------:|-------------:|--------------:|------------:|
    ///| ReadFileByteByByte | 155,190.13 us | 5,098.106 us | 15,031.880 us |     4.45 KB |
    ///|      ReadFileBytes |   6,483.61 us |   157.987 us |    465.828 us |    10938 KB | <<< baseline
    ///|       ReadFileText |  93,778.67 us | 2,460.125 us |  6,978.965 us | 43567.99 KB |
    ///|       ReadDatabase |   6,700.00 us |   174.500 us |    511.800 us |    10938 KB |  1.12 |     0.10 |        1.00 |
    [MemoryDiagnoser(false)]
    public class GeoDataBenchmark
    {
        [Benchmark(Baseline =true)]
        public void ReadFileBytes()
        {
            var q = System.IO.File.ReadAllBytes("geobase.dat");
        }

        [Benchmark]
        public void ReadDatabase()
        {
            var q = new GeoData.Db.GeoIp();
        }
    }
}
