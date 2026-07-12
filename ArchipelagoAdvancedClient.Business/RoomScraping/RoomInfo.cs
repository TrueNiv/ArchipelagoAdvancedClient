namespace ArchipelagoAdvancedClient.Business.RoomScraping;

public class RoomInfo
{
    public string Host { get; set; }
    public string Port { get; set; }
    public List<SlotInfo> Slots { get; set; }
}