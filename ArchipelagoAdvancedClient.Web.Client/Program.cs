using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArchipelagoAdvancedClient.Shared;
using ArchipelagoAdvancedClient.Shared.Services;
using ArchipelagoAdvancedClient.Web.Client.Services;

namespace ArchipelagoAdvancedClient.Web.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<Routes>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Add device-specific services used by the ArchipelagoAdvancedClient.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();

        await builder.Build().RunAsync();
    }
}