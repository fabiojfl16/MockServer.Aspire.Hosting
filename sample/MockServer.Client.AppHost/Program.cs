using MockServer.Aspire.Hosting;
using MockServer.Client.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mockserver = builder.AddMockServer("mockserver", 1080)
    .WithExpectation(MockServerExpectations.WheatherForecastExpectation);

builder.AddProject<Projects.MockServer_Client_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(mockserver);

builder.Build().Run();
