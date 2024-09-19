using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

namespace MockServer.Aspire.Hosting;

public static class MockServerResourceBuilderExtensions
{
    public static IResourceBuilder<MockServerResource> AddMockServer(
        this IDistributedApplicationBuilder builder,
        string name,
        int httpPort = 1080)
    {
        var resource = new MockServerResource(name);

        return builder.AddResource(resource)
                      .WithImage("mockserver/mockserver")
                      .WithImageRegistry("docker.io")
                      .WithHttpEndpoint(httpPort, 1080);
    }

    public static IResourceBuilder<MockServerResource> WithExpectations(this IResourceBuilder<MockServerResource> builder, List<string> expectations)
    {
        builder.Resource.Expectations.AddRange(expectations);
        builder.ApplicationBuilder.Services.AddLifecycleHook<MockServerConfigHook>();
        return builder;
    }
}
