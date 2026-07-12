using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArchipelagoAdvancedClient.Business;
using ArchipelagoAdvancedClient.Business.APConnector;
using ArchipelagoAdvancedClient.Business.RoomScraping;
using ArchipelagoAdvancedClient.Shared;
using ArchipelagoAdvancedClient.Shared.Services;
using ArchipelagoAdvancedClient.Web.Client.Services;

namespace ArchipelagoAdvancedClient.Web.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // This assembly boots both as the standalone GitHub Pages app (its own index.html has a
        // literal <div id="app">) and, unmodified, as the WASM runtime for the
        // ArchipelagoAdvancedClient.Web ASP.NET host (whose App.razor has no #app element - root
        // components there attach automatically via SSR-embedded markers instead). Registering a
        // root component against a selector that doesn't exist throws during RunAsync() and breaks
        // ALL interactivity app-wide, so only do this when actually running standalone. The
        // GitHub Pages base href is what distinguishes the two at runtime.
        if (builder.HostEnvironment.BaseAddress.Contains("/ArchipelagoAdvancedClient/"))
        {
            builder.RootComponents.Add<Routes>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }

        // Add device-specific services used by the ArchipelagoAdvancedClient.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddScoped<ILocalStorageService, BrowserLocalStorageService>();
        builder.Services.AddScoped<IExternalLinkOpener, BrowserExternalLinkOpener>();

        // Add business logic services used by the ArchipelagoAdvancedClient.Shared project
        builder.Services.AddSingleton<IChatService, ChatService>();
        builder.Services.AddSingleton<IArchipelagoConnectorService, ArchipelagoConnectorService>();
        builder.Services.AddSingleton<IRoomScraperService, RoomScraperService>();

        await builder.Build().RunAsync();
    }
}