using ArchipelagoAdvancedClient.Services;
using ArchipelagoAdvancedClient.Shared;
using ArchipelagoAdvancedClient.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace ArchipelagoAdvancedClient;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.Services.AddLogging();

        // Add device-specific services used by the ArchipelagoAdvancedClient.Shared project
        appBuilder.Services.AddSingleton<IFormFactor, FormFactor>();

        appBuilder.RootComponents.Add<Routes>("#app");

        var app = appBuilder.Build();

        // Photino validates this path lazily, against whatever the process's working
        // directory happens to be when Run() starts - which differs between `dotnet run`,
        // running the built exe directly, and Rider's own working-directory setting. Use an
        // absolute path so it can't depend on how/where the process was launched from.
        var iconFile = Path.Combine(AppContext.BaseDirectory, "wwwroot", "favicon.ico");

        app.MainWindow
            .SetIconFile(iconFile)
            .SetTitle("ArchipelagoAdvancedClient");

#if DEBUG
        app.MainWindow
            .SetDevToolsEnabled(true)
            .SetContextMenuEnabled(true);
#endif

        AppDomain.CurrentDomain.UnhandledException += (_, error) =>
        {
            // Not calling app.MainWindow.ShowMessage here: it's a native GTK/WebKit call and this
            // handler can fire on an arbitrary background thread, which crashes the native call
            // (GTK UI calls are only safe from the main thread) and masks the real exception.
            Console.Error.WriteLine("Fatal exception: " + error.ExceptionObject);
        };

        app.Run();
    }
}
