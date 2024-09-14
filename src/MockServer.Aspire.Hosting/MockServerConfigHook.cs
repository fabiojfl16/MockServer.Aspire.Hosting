using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using System.Text;

namespace MockServer.Aspire.Hosting;

internal class MockServerConfigHook : IDistributedApplicationLifecycleHook
{
    public async Task AfterResourcesCreatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken)
    {
        var mockServerInstances = appModel.Resources.OfType<MockServerResource>();

        var url = "http://localhost:1080/mockserver/expectation";
        using var client = new HttpClient();

        foreach (var mockServerInstance in mockServerInstances)
        {
            foreach (var expectation in mockServerInstance.Expectations)
            {
                var content = new StringContent(expectation, Encoding.UTF8, "application/json");
                await client.PutAsync(url, content, cancellationToken);
            }
        }
    }
}
