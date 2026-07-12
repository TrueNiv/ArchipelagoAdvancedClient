using System.Text.Json;
using ArchipelagoAdvancedClient.Shared.Services;

namespace ArchipelagoAdvancedClient.Services;

public class FileLocalStorageService : ILocalStorageService
{
    private static readonly string StorageDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ArchipelagoAdvancedClient");

    public async Task SetItemAsync<T>(string key, T value)
    {
        Directory.CreateDirectory(StorageDirectory);
        var json = JsonSerializer.Serialize(value);
        await File.WriteAllTextAsync(GetPath(key), json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var path = GetPath(key);
        if (!File.Exists(path))
            return default;

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    public Task RemoveItemAsync(string key)
    {
        var path = GetPath(key);
        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }

    private static string GetPath(string key) => Path.Combine(StorageDirectory, $"{key}.json");
}
