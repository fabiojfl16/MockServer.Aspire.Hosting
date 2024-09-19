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
        // The AddResource method is a core API within .NET Aspire and is
        // used by resource developers to wrap a custom resource in an
        // IResourceBuilder<T> instance. Extension methods to customize
        // the resource (if any exist) target the builder interface.
        var resource = new MockServerResource(name);

        var resourceBuilder = builder.AddResource(resource)
                      .WithImage("mockserver/mockserver")
                      .WithImageRegistry("docker.io")
                      .WithHttpEndpoint(httpPort, 1080);

        return resourceBuilder;
    }

    public static IResourceBuilder<MockServerResource> WithExpectation(this IResourceBuilder<MockServerResource> builder, string expectation)
    {
        builder.Resource.Expectations.Add(expectation);
        builder.ApplicationBuilder.Services.TryAddLifecycleHook<MockServerConfigHook>();
        return builder;
    }
}
