# Test task:
Make a web app capable of returning user coords by IP and a list of places for given city.

## Tools:
- C#, ASP.NET Core
- MS Visual Studio
- HTML5, CSS3, JavaScript

## Architecture and source code requirements:
- The app design must be capable to provide handling of 10 000 000 unique users per day and 100 000 000 queries per day.
- User interface must be a Single Page Application, as lightweight as it is possible, and should be written on JavaScript without using of 3-rd party SPA frameworks.

## Tech requirements:
- Database should be loaded in memory at application start, in one thread and without any parallelism whatsoever.
- Database load time should not differ from file reading time more than on 5 ms on data parsing, index creation and any other possible overheads.
- The database must use fast binary search for both acquiring location by IP and list of places by city.
- The app must expose two HTTP API methods:
```
  GET /ip/location?ip=123.234.123.234
  GET /city/locations?city=cit_Gbqw4
```
- UI must be a SPA and must be made of two parts, a menu in left side and a screen on the right side.

The complete [task description](https://www.metaquotes.net/ru/company/vacancies/tests/dot-net)

# Solution description:
- Main part is GeoData/GeoData.csproj, can be launched by "dotnet run" command. 
- For development "npm run watch" can be called in GeoData/GeoApp folder to allow hot reloads. Some tweaking of webpack might be required depending on settings of Visual Studio.
- The Benchmarks folder contains database load test and results
```
|        Method |          Mean |        Error |        StdDev |   Allocated | Ratio | Ratio SD | Alloc Ratio |
|-------------- |--------------:|-------------:|--------------:|------------:|
| ReadFileBytes |   6,483.61 us |   157.987 us |    465.828 us |    10938 KB | <<< baseline
|  ReadFileText |  93,778.67 us | 2,460.125 us |  6,978.965 us | 43567.99 KB |
|  ReadDatabase |   6,700.00 us |   174.500 us |    511.800 us |    10938 KB |  1.12 |     0.10 |        1.00 |
```
- UnitTests and IntegrationTests cover the main part of .NetCore app to an extent to make futher changes and tweaks easy
- UI is in /GeoData/wwwroot folder, exposed to Kestrel server by   .UseDefaultFiles().UseStaticFiles() expression. Entry point is Index.html page that includes the js/app.js module which is build by Webpack from /GeoData/GeoApp SPA.
- The idea for UI credits to [JeremyLikness](https://github.com/JeremyLikness/vanillajs-deck/) that at some measure stretches the requirement of not using 3rd party frameworks.
- The idea to use the JSX without React credits to [Kartik Nair](https://dev.to/kartiknair)

## The UI structure
index.html includes app.js which sets up both web components for two-parts UI 
```
  <screen-controls deck="main"> --- </screen-controls>
  <screen-deck id="main" start="home">
    <h1>Клиентская часть приложения. Выполнена в идеологии Single Page Application.</h1>
    <h2>Идёт загрузка ...</h2>
  </screen-deck>
```	

app.js includes components necessary to
```
--navigator.js
  --router.js
  --screens/*.js
    --templates/*.js
--controls.js
  --navigator.js
-- component.js
```
Base point is component which contains a simple pragma for JSX constructor function. 

The screens are modules that export constructor for initial element creation, state management funciton for cases when they are displayed. Also screen modules contain necessary lifetime functions.  

The router.js module is responsible for handling routing. It has two primary functions:
- Setting the route (hash) to correspond to the current screen;
- Responding to navigation by raising a custom event to inform subscribers that the route has changed.

The navigator.js is the “main module” that displays the deck. Screens are registered in navigator. It is responsible for showing screens and handling movement between screens. Its module expose itself as a reusable web component. 

The last module, also a web component, is the controls for the deck. The module is plugged into the web component lifecycle to load the template for the contols and wire in event listeners of parent navigator to display screens.

## Load requrement
The handling of 10 000 000 unique users per day (U) and 100 000 000 queries (Q) per day.
According to the [techempower benchmarks](https://www.techempower.com/benchmarks/#section=test&runid=8ca46892-e46c-4088-9443-05722ad6f7fb&hw=ph&test=plaintext) this  well within Kestel server capabilities (7 million per second!) so all blockers can be in controller code.

Consider user session start is a complete load of html page incluing scripts and styles and a request to HTTP API methods.

The user performs about (Q / U) = 10 API queries per session. We dont have user reaction time, so we'l try to model it, consider it as something random from as fast as 500 ms to as long as 1 minute.
Session length becomes 5 s to 10 minutes. Average session length ~5 min

The hourly users amount can be calculated as U users per day / 24 hrs per day ~ 420K. Consider / 60 minute in hour / 5 min session duration ~ 35 0000 concurrent users.  Peak can be as high as 35 000 simultaneous requests (low probability though), still within theoretical server capabilities, but too much to my dev environment (see below). Sharding can be used to serve peak loads.

Average requests can be 420K users per hour \* 10 requests / 3600 sec in hour ~ 1200 request per second. Development environment is capable to serve peaks about 3 to 4 times over this.

### Netling test of city controller
Netling Running 10s test with 1024 threads @ http://localhost:5000/city/locations?city=cit_Erupedebefevy%20O

```
39924 requests in 10.22s
    Requests/sec:   3905
    Bandwidth:      77 mbit
    Errors:         0
Latency
    Median:         32.111 ms
    StdDev:         92.587 ms
    Min:            8.439 ms
    Max:            428.365 ms

  █
 ██
 ███
 ███
 ████                                       ██
 █████                                      ██
█████████████████████████ ████ ████████████████  █  █████ ████ █        █      █
8.439 ms ===========================================================  428.365 ms
```
### Netling test of location controller
Running 10s test with 1024 threads @ http://localhost:5000/ip/location?ip=116.226.107.115
```
46376 requests in 10.08s
    Requests/sec:   4599
    Bandwidth:      11 mbit
    Errors:         0
Latency
    Median:         103.356 ms
    StdDev:         32.780 ms
    Min:            10.705 ms
    Max:            222.361 ms

                                   █
                                   █
                                  ██
                                  ███
                                  ████
                              █ █ █████
███████████████████████████████████████████████████████ █ █ ██ █  █ █ █  ███   █
10.705 ms =========================================================== 222.361 ms
```