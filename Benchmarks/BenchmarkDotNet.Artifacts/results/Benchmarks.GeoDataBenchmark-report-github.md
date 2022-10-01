``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.18363.1679/1909/November2019Update/19H2)
Intel Core i3-6006U CPU 2.00GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=5.0.406
  [Host]     : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2
  DefaultJob : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT AVX2


```
|        Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| ReadFileBytes |  6.146 ms | 0.1575 ms | 0.4643 ms |  1.00 |    0.00 |  10.68 MB |        1.00 |
|  ReadDatabase | 13.288 ms | 0.4172 ms | 1.2037 ms |  2.18 |    0.25 |  21.36 MB |        2.00 |
