using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ArchipelagoAdvancedClient.Business.RoomScraping;

public class RoomScraperService : IRoomScraperService
{
    private readonly HttpClient _client = new();

    public async Task<RoomInfo> ScrapeRoom(string url)
    {
        var roomUri = new Uri(url);
        var roomId = roomUri.Segments[^1].Trim('/');

        var apiUrl = $"{roomUri.Scheme}://{roomUri.Host}/api/room_status/{roomId}";
        var status = await _client.GetFromJsonAsync<RoomStatusResponse>(apiUrl);

        if (status is null) return new RoomInfo();

        return new RoomInfo
        {
            Host = roomUri.Host,
            Port = status.LastPort.ToString(),
            Slots = status.Players.Select(player => new SlotInfo(player[0], player[1])).ToList()
        };
    }

    private class RoomStatusResponse
    {
        [JsonPropertyName("last_port")]
        public int LastPort { get; set; }

        [JsonPropertyName("players")]
        public List<string[]> Players { get; set; } = [];
    }
}
