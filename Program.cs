using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueDEMO.AzureQueueStorage;
using QueueDEMO.QueueInterfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IAzureStorageQueue, AzureStorageQueue>();
    })
    .Build();

host.Run();