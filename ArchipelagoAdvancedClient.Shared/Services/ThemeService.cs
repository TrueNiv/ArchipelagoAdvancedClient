namespace ArchipelagoAdvancedClient.Shared.Services;

public class ThemeService(ILocalStorageService localStorage) : IThemeService
{
    private const string StorageKey = "dark-mode";

    public bool IsDarkMode { get; private set; } = true;

    public event Action? ThemeChanged;

    public async Task InitializeAsync()
    {
        var stored = await localStorage.GetItemAsync<bool?>(StorageKey);
        if (stored.HasValue && stored.Value != IsDarkMode)
        {
            IsDarkMode = stored.Value;
            ThemeChanged?.Invoke();
        }
    }

    public async Task SetDarkModeAsync(bool isDarkMode)
    {
        if (IsDarkMode == isDarkMode) return;

        IsDarkMode = isDarkMode;
        await localStorage.SetItemAsync(StorageKey, isDarkMode);
        ThemeChanged?.Invoke();
    }
}
