namespace ArchipelagoAdvancedClient.Shared.Services;

public interface IThemeService
{
    bool IsDarkMode { get; }
    event Action? ThemeChanged;

    /// <summary>
    /// Loads the persisted theme preference, if any. Must only be called from OnAfterRenderAsync
    /// (or later) - see ILocalStorageService/BrowserLocalStorageService for why.
    /// </summary>
    Task InitializeAsync();

    Task SetDarkModeAsync(bool isDarkMode);
}
