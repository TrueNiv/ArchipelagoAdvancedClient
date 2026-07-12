using System.Text.Json;
using Microsoft.JSInterop;

namespace ArchipelagoAdvancedClient.Shared.Services;

// Backs both Web.Client and Web: both render their component tree as WebAssembly in the
// browser, so browser localStorage is available in either case. Note that ArchipelagoAdvancedClient.Web
// prerenders on the server first, where no JS runtime exists yet - only call this from
// OnAfterRenderAsync (or later), never from OnInitializedAsync, or it'll throw.
public class BrowserLocalStorageService(IJSRuntime jsRuntime) : ILocalStorageService
{
    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveItemAsync(string key)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
