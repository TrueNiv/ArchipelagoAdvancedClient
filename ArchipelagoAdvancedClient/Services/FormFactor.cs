using ArchipelagoAdvancedClient.Shared.Services;

namespace ArchipelagoAdvancedClient.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "Desktop";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}
