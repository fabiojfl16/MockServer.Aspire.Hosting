namespace Aspire.Hosting.ApplicationModel;

public sealed class MockServerResource(string name) : ContainerResource(name), IResourceWithServiceDiscovery
{
    internal List<string> Expectations { get; set; } = [];
}