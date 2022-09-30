``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.18363.1679/1909/November2019Update/19H2)
Intel Core i3-6006U CPU 2.00GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=5.0.406
  [Host]     : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2
  DefaultJob : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2


```
|       Method |     Mean |    Error |   StdDev |   Median | Allocated |
|------------- |---------:|---------:|---------:|---------:|----------:|
| ReadDatabase | 390.9 ms | 13.60 ms | 39.24 ms | 380.3 ms |  73.24 MB |
