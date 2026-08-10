using System.Text.Json;
using ArchipelagoAdvancedClient.Shared.Services;

namespace ArchipelagoAdvancedClient.Services;

public class FileLocalStorageService : ILocalStorageService
{
    private static readonly string StorageDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ArchipelagoAdvancedClient");

    private static readonly string SettingsPath = Path.Combine(StorageDirectory, "settings.json");

    // Read-modify-write of the shared file isn't atomic, so overlapping calls (e.g. a setting
    // saved on every keystroke) could otherwise clobber each other's writes.
    private readonly SemaphoreSlim gate = new(1, 1);

    public async Task SetItemAsync<T>(string key, T value)
    {
        await gate.WaitAsync();
        try
        {
            var all = await LoadAllAsync();
            all[key] = JsonSerializer.SerializeToElement(value);
            await SaveAllAsync(all);
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        await gate.WaitAsync();
        try
        {
            var all = await LoadAllAsync();
            return all.TryGetValue(key, out var element) ? element.Deserialize<T>() : default;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        await gate.WaitAsync();
        try
        {
            var all = await LoadAllAsync();
            if (all.Remove(key))
            {
                await SaveAllAsync(all);
            }
        }
        finally
        {
            gate.Release();
        }
    }

    private static async Task<Dictionary<string, JsonElement>> LoadAllAsync()
    {
        if (!File.Exists(SettingsPath))
            return [];

        var json = await File.ReadAllTextAsync(SettingsPath);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? [];
    }

    private static async Task SaveAllAsync(Dictionary<string, JsonElement> all)
    {
        Directory.CreateDirectory(StorageDirectory);
        await File.WriteAllTextAsync(SettingsPath, JsonSerializer.Serialize(all));
    }
}
