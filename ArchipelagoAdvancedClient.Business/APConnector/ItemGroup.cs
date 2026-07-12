namespace ArchipelagoAdvancedClient.Business.APConnector;

public class ItemGroup
{
    public string Name { get; set; }
    public string[] Items { get; set; }
    
    public ItemGroup(string name, string[] items)
    {
        Name = name;
        Items = items;
    }
}