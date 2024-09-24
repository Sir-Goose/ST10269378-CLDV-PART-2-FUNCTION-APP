// configures the azure functions host
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.WebJobs.Extensions.Storage.Blobs;
using Microsoft.Azure.WebJobs.Extensions.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

// creates a new HostBuilder instance for configuring the azure functions host
var host = new HostBuilder()
    // configures the azure functions web application
    .ConfigureFunctionsWebApplication()
    // configures the services for the azure functions host
    .ConfigureServices(services =>
    {
        // adds application insights telemetry for the azure functions worker service
        services.AddApplicationInsightsTelemetryWorkerService();
        // configures the application insights for the azure functions
        services.ConfigureFunctionsApplicationInsights();
    })
    // configures the web jobs for the azure functions host
    .ConfigureWebJobs(b =>
    {
        // registers the HTTP binding for the azure functions
        b.AddHttp();
        // registers the Azure Storage blob binding for the azure functions
        b.AddAzureStorageBlobs(); // For Blob Storage functions
        // registers the Azure Storage queue binding for the azure functions
        b.AddAzureStorageQueues(); // For Queue Storage functions
        // b.AddAzureStorageQueuesScaleForTrigger(); // Add this if scaling is needed for Queue Triggers
    })
    // builds the azure functions host
    .Build();

// runs the azure functions host
host.Run();