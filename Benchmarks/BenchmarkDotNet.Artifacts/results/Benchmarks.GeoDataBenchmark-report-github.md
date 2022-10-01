``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.18363.1679/1909/November2019Update/19H2)
Intel Core i3-6006U CPU 2.00GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=5.0.406
  [Host]     : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2
  DefaultJob : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2


```
|        Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |---------:|----------:|----------:|------:|--------:|----------:|------------:|
| ReadFileBytes | 5.924 ms | 0.1156 ms | 0.2656 ms |  1.00 |    0.00 |  10.68 MB |        1.00 |
|  ReadDatabase | 6.700 ms | 0.1745 ms | 0.5118 ms |  1.12 |    0.10 |  10.68 MB |        1.00 |
