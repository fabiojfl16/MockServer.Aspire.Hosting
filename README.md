# MockServer.Aspire.Hosting
An extension to use [MockServer](https://www.mock-server.com/) container with [.NET Aspire](https://github.com/dotnet/aspire). This is a great tool to mock external api dependencies and also for Integration tests implementation.

Install the **MockServer.Aspire.Hosting** NuGet Package in the **.AppHost** project of your solution, and add the following code in the Program.cs file
```csharp
var mockserver = builder.AddMockServer("mockserver", 1080);
```

The **AddMockServer** method takes two parameters
- The **name** used by Service Discovery: http://mockserver
- The **port** MockServer should run on. The default port is 1080

&nbsp;

Notice the **.WithExpectations** extension method. Here you can add a list of expectations that mockserver should respond
```csharp
var mockserver = builder.AddMockServer("mockserver", 1080)
    .WithExpectations([MockServerExpectations.WheatherForecastExpectation]);
```

For instance, this is the response mockserver should return for the GET method http://mockserver/api/weatherforecast
```json
"httpRequest": {
	"method": "GET",
	"path": "/api/weatherforecast",
	"queryStringParameters": {},
	"cookies": {}
},
"httpResponse": {
	"body": 
	[
		{"Date":"2024-09-14","TemperatureC":42,"Summary":"Sweltering","TemperatureF":107},
		{"Date":"2024-09-15","TemperatureC":-3,"Summary":"Warm","TemperatureF":27},
		{"Date":"2024-09-16","TemperatureC":43,"Summary":"Freezing","TemperatureF":109},
		{"Date":"2024-09-17","TemperatureC":9,"Summary":"Cool","TemperatureF":48},
		{"Date":"2024-09-18","TemperatureC":27,"Summary":"Hot","TemperatureF":80}
	]
}
```

After adding mockserver, you should include it as a reference for your Api

```csharp
builder.AddProject<Projects.MockServer_Client_Web>("webfrontend")
    .WithReference(mockserver);
```

Take a look at the sample project for better understanding.
