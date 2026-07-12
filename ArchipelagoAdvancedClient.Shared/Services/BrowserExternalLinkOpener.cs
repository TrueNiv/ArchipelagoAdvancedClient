using Microsoft.JSInterop;

namespace ArchipelagoAdvancedClient.Shared.Services;

// Backs both Web.Client and Web: both render their component tree as WebAssembly in the
// browser, so window.open is available in either case.
public class BrowserExternalLinkOpener(IJSRuntime jsRuntime) : IExternalLinkOpener
{
    public async Task OpenAsync(string url)
    {
        await jsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}
