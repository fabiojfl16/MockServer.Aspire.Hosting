using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using Polly.Retry;
using Polly;
using System.Text;

namespace MockServer.Aspire.Hosting;

internal class MockServerConfigHook : IDistributedApplicationLifecycleHook
{
    public async Task AfterResourcesCreatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken)
    {
        var resources = appModel.Resources.OfType<MockServerResource>();

        if (!resources.Any())
            return;

        using var httpClient = new HttpClient();

        foreach (var resource in resources)
        {
            var endpoint = resource.GetEndpoint("http");
            var url = $"{endpoint.Url}/mockserver/expectation";

            await CheckStatusWithRetryAsync($"{endpoint.Url}/status", new StringContent(string.Empty), httpClient, 3);

            foreach (var expectation in resource.Expectations)
            {
                var content = new StringContent(expectation, Encoding.UTF8, "application/json");
                await httpClient.PutAsync(url, content, cancellationToken);
            }
        }
    }

    public static async Task<HttpResponseMessage> CheckStatusWithRetryAsync(string url, HttpContent content, HttpClient httpClient, int maxRetries = 3)
    {
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                (response, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Request to MockServer failed. Waiting {timeSpan} before retry {retryCount}");
                });

        return await retryPolicy.ExecuteAsync(() => httpClient.PutAsync(url, content));
    }
}
