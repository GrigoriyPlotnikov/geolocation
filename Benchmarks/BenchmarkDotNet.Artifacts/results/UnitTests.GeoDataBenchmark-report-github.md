``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.18363.1679/1909/November2019Update/19H2)
Intel Core i3-6006U CPU 2.00GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=5.0.406
  [Host]     : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2
  DefaultJob : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2


```
|                 Method |          Mean |        Error |        StdDev |        Median | Allocated |
|----------------------- |--------------:|-------------:|--------------:|--------------:|----------:|
| ReadFileReferencePoint | 139,821.85 μs | 5,091.765 μs | 14,772.135 μs | 135,485.98 μs |   4.45 KB |
|           ReadDatabase |      75.00 μs |     1.499 μs |      3.443 μs |      74.81 μs |   4.66 KB |
