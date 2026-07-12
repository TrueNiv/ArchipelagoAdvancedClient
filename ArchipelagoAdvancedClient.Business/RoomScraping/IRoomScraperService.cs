namespace ArchipelagoAdvancedClient.Business.RoomScraping;

public interface IRoomScraperService
{
    Task<RoomInfo> ScrapeRoom(string url);
}
