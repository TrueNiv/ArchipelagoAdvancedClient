namespace ArchipelagoAdvancedClient.Shared.Models;

public class ChatConnectionSettings
{
    public string Url { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Game { get; set; } = string.Empty;
    public bool OverrideVersion { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; } = 7;
    public int Build { get; set; }
}
