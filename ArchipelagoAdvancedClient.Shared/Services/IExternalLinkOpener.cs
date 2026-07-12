namespace ArchipelagoAdvancedClient.Shared.Services;

public interface IExternalLinkOpener
{
    Task OpenAsync(string url);
}
