using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;

namespace ArchipelagoAdvancedClient.Business.APConnector;

public interface IArchipelagoConnectorService
{
    string ConnectionMessage { get; }
    LoginResult LoginResult { get; }
    EventHandler<List<HintDTO>> HintsChanged { get; set; }
    IReadOnlyList<HintDTO> Hints { get; }
    ArchipelagoSession ArchipelagoSession { get; }
    DeathLinkService DeathLinkService { get; }
    Dictionary<string, string[]>? ItemGroups { get; }

    string? FindPlayer(int id);
    LocationDTO? FindLocation(long id, string? game = null);
    string? FindItem(long id, string? game = null);
    string? FindGame(int id);
    List<LocationDTO?> ListMyLocations();
    Task Connect(string url, string game, string name, string password, Version? version = null);
    Task Disconnect();
    Task SendChatMessage(string message);
    Task HintLocation(long locationId);
    Task<string?> PeekLocation(long locationId);
    Task SendLocation(long locationId);
    Task SendGoal();
    Task ReleaseAllRemainingLocations();
}
