using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using System.Text;

namespace MockServer.Aspire.Hosting;

internal class MockServerConfigHook : IDistributedApplicationLifecycleHook
{
    public async Task AfterResourcesCreatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken)
    {
        var resources = appModel.Resources.OfType<MockServerResource>();

        using var client = new HttpClient();

        foreach (var resource in resources)
        {
            foreach (var expectation in resource.Expectations)
            {
                var endpoint = resource.GetEndpoint("http");
                var url = $"{endpoint.Url}/mockserver/expectation";

                var content = new StringContent(expectation, Encoding.UTF8, "application/json");
                await client.PutAsync(url, content, cancellationToken);
            }
        }
    }
}
