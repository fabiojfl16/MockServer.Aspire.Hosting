namespace MockServer.Client.AppHost;

public class MockServerExpectations
{
    public const string WheatherForecastExpectation = @"
	{
		""httpRequest"": {
			""method"": ""GET"",
			""path"": ""/api/weatherforecast"",
			""queryStringParameters"": {},
			""cookies"": {}
		},
		""httpResponse"": {
			""body"": 
			[
				{""Date"":""2024-09-14"",""TemperatureC"":42,""Summary"":""Sweltering"",""TemperatureF"":107},
				{""Date"":""2024-09-15"",""TemperatureC"":-3,""Summary"":""Warm"",""TemperatureF"":27},
				{""Date"":""2024-09-16"",""TemperatureC"":43,""Summary"":""Freezing"",""TemperatureF"":109},
				{""Date"":""2024-09-17"",""TemperatureC"":9,""Summary"":""Cool"",""TemperatureF"":48},
				{""Date"":""2024-09-18"",""TemperatureC"":27,""Summary"":""Hot"",""TemperatureF"":80}
			]
		}
	}";
}
