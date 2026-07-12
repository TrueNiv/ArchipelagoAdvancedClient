using ArchipelagoAdvancedClient.Business;
using ArchipelagoAdvancedClient.Business.APConnector;
using ArchipelagoAdvancedClient.Business.RoomScraping;
using ArchipelagoAdvancedClient.Web.Components;
using ArchipelagoAdvancedClient.Shared.Services;
using ArchipelagoAdvancedClient.Web.Services;

namespace ArchipelagoAdvancedClient;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        // Add device-specific services used by the ArchipelagoAdvancedClient.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddScoped<ILocalStorageService, BrowserLocalStorageService>();
        builder.Services.AddScoped<IExternalLinkOpener, BrowserExternalLinkOpener>();

        // Add business logic services used by the ArchipelagoAdvancedClient.Shared project
        builder.Services.AddSingleton<IChatService, ChatService>();
        builder.Services.AddSingleton<IArchipelagoConnectorService, ArchipelagoConnectorService>();
        builder.Services.AddSingleton<IRoomScraperService, RoomScraperService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(
                typeof(ArchipelagoAdvancedClient.Shared._Imports).Assembly,
                typeof(ArchipelagoAdvancedClient.Web.Client._Imports).Assembly);

        app.Run();
    }
}