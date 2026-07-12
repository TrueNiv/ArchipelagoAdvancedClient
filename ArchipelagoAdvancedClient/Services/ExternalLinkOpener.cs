using System.Diagnostics;
using ArchipelagoAdvancedClient.Shared.Services;

namespace ArchipelagoAdvancedClient.Services;

// Photino's webview has no popup/new-window handler, so target="_blank" links are a no-op there.
// Shell out to the OS's registered URL handler (xdg-open on Linux, the shell association on Windows) instead.
public class ExternalLinkOpener : IExternalLinkOpener
{
    public Task OpenAsync(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        return Task.CompletedTask;
    }
}
