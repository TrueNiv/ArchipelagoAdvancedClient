namespace ArchipelagoAdvancedClient.Business.RoomScraping;

public class SlotInfo
{
    public SlotInfo(string name, string game)
    {
        Name = name;
        Game = game;
    }
    
    public string Name { get; set; }
    public string Game { get; set; }
}