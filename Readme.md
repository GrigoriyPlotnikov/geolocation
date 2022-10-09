Test task:
Make a web app capable of returning user coords by IP and a list of places for given city.

Tools:
- C#, ASP.NET Core
- MS Visual Studio
- HTML5, CSS3, JavaScript

Architecture and source code requirements:
- The app design must be capable to provide handling of 10 000 000 unique users per day and 100 000 000 queries per day.
- User interface must be a Single Page Application, as lightweight as it is possible, and should be written on JavaScript without using of 3-rd party SPA frameworks.

Tech requirements:
- Database should be loaded in memory at application start, in one thread and without any parallelism whatsoever.
- Database load time should not differ from file reading time more than on 5 ms on data parsing, index creation and any other possible overheads.
- The database must use fast binary search for both acquiring location by IP and list of places by city.
- The app must expose two HTTP API methods:
	GET /ip/location?ip=123.234.123.234
	GET /city/locations?city=cit_Gbqw4
- UI must be a SPA and must be made of two parts, a menu in left side and a screen on the right side.

The complete [task description](https://www.metaquotes.net/ru/company/vacancies/tests/dot-net)

Solution description:
- Main part is GeoData/GeoData.csproj, can be launched by "dotnet run" command
- The Benchmarks folder contains database load test and results
   |             Method |          Mean |        Error |        StdDev |   Allocated | Ratio | Ratio SD | Alloc Ratio |
   |------------------- |--------------:|-------------:|--------------:|------------:|
   |      ReadFileBytes |   6,483.61 us |   157.987 us |    465.828 us |    10938 KB | <<< baseline
   |       ReadFileText |  93,778.67 us | 2,460.125 us |  6,978.965 us | 43567.99 KB |
   |       ReadDatabase |   6,700.00 us |   174.500 us |    511.800 us |    10938 KB |  1.12 |     0.10 |        1.00 |
- UnitTests and IntegrationTests cover the main part of .NetCore app to an extent to make futher changes and tweaks easy
- UI is in \GeoData\wwwroot folder, exposed to Kestrel server by   .UseDefaultFiles().UseStaticFiles() expression. Entry point is Index.html page that includes the js/app.js module. Webpack is yet to be attached to serve page UI in one request.
- The idea for UI credits to [JeremyLikness](https://github.com/JeremyLikness/vanillajs-deck/) that at some measure stretches the requirement of not using 3rd party frameworks 

#The UI structure
index.html sets up both web components for two-parts UI 
	<screen-controls deck="main"> --- </screen-controls>
	<screen-deck id="main" start="home">
		<h1>Клиентская часть приложения. Выполнена в идеологии Single Page Application.</h1>
		<h2>Идёт загрузка ...</h2>
	</screen-deck>
	
app.js includes components necessary to
--navigator.js
	--router.js
	--screen.js
		--dataBinding.js
			--observable.js
	--loadScreens.js
		--screen.js
		--dataBinding.js
			--observable.js
	--animator.js 
--controls.js
	--navigator.js
	--dataBinding.js		

Base point is observable which contains a simple implementation of the observer pattern. A class wraps a value and notifies subscribers when the value changes. A computed observable is available that can handle values derived from other observables (for example, the result of an equation where the variables are being observed). 

The databinding.js module provides databinding services to the application. The pair of methods execute and executeInContext are used to evaluate screens with a designated this. Essentially, each “screen” has a context that is used for setting up expressions for databinding, and the scripts included in the screen are run in that context. The context is defined in the “screen” class that will be explored later.
In the HTML, the databinding for n1 is declared like this:
<label for="first">
   <div>Number:</div>
   <input type="text" id="first" data-bind="n1"/>
</label>
In the script tag it is set up like this:
	const n1 = this.observable(2);
	this.n1 = n1;

The Screen class in screen.js is holds the information that represents a “screen” in the app. Main screen content is formed in the constructor, async dataBindExecute must be called later to perform possible async actions in the screen scripts. 

The router.js module is responsible for handling routing. It has two primary functions:
- Setting the route (hash) to correspond to the current screen;
- Responding to navigation by raising a custom event to inform subscribers that the route has changed.

The loadScreens part is the place where screens are registered and loaded.

The navigator.js is the “main module” that displays the deck. It is responsible for showing screens and handling movement between screens. This is the first module we will examine to expose itself as a reusable web component. 

The last module, also a web component, is the controls for the deck. The module is plugged into the web component lifecycle to load the template for the contols and wire in event listeners of parent navigator to display screens.

#Load requrement
The handling of 10 000 000 unique users per day (U) and 100 000 000 queries (Q) per day.
Consider query is a complete load of page incluing UI and a request to HTTP API methods.
The user performs about (Q / U) = 10 queries per session, which fits in a session about 1 minute long. 
The concurrent users amount per minute can be calculated as U users per day / 24 hrs per day / 60 minute in hour ~ 7000 per minute mean.
Supposing normal distribution 
Total requests can be as big as ~ 115 users per second