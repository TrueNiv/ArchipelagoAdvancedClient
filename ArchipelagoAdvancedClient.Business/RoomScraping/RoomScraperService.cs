using HtmlAgilityPack;

namespace ArchipelagoAdvancedClient.Business.RoomScraping;

public class RoomScraperService : IRoomScraperService
{
    private readonly HttpClient _client = new();

    public async Task<RoomInfo> ScrapeRoom(string url)
    {
        var html = await _client.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);


        var room = new RoomInfo{Slots = []};

        var connectionString = doc.DocumentNode
            .SelectNodes(@"//*[@id=""host-room-info""]/span")
            .Descendants()
            .FirstOrDefault()?.InnerText;

        if (connectionString is null) return new RoomInfo();

        connectionString = connectionString.Trim().Trim('\'', '"').Replace("/connect ", "").Trim();

        var data = connectionString.Split(':');
        room.Host = data[0];
        room.Port = data[1];

        var rows = doc.DocumentNode.SelectNodes(@"//*[@id=""slots-table""]/tbody/tr");
        if (rows is null) return room;

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td");
            if (cells is null || cells.Count < 3) continue;

            var player = cells[1].SelectSingleNode(".//a")?.InnerText.Trim();
            var game = cells[2].InnerText.Trim();

            if (player is not null)
                room.Slots.Add(new SlotInfo( player, game));
        }

        return room;
    }
}
