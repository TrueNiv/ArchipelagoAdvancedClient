using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;

namespace ArchipelagoAdvancedClient.Business.APConnector;

public class ArchipelagoConnectorService : IArchipelagoConnectorService
{
    public static readonly string[] DEFAULT_TAGS = ["TextOnly", "AP"];

    private readonly IChatService _chatService;

    private string _url;
    private string _game;
    private string _name;
    private string _password;
    private Version _version { get; set; }

    public string ConnectionMessage { get; private set; }
    public LoginResult LoginResult { get; private set; }

    public EventHandler<List<HintDTO>> HintsChanged { get; set; }

    public ArchipelagoSession ArchipelagoSession {get; private set;}
    public DeathLinkService DeathLinkService { get; private set; }
    public Dictionary<string, string[]>? ItemGroups { get; private set; }

    private Dictionary<int, string> players = new();
    private Dictionary<(long id, string game), LocationDTO> locations = new();
    private Dictionary<(long id, string game), string> items = new();
    private Dictionary<int, string> games = new();
    private List<HintDTO> hints = [];

    public ArchipelagoConnectorService(IChatService chatService)
    {
        _chatService = chatService;
    }

    public string? FindPlayer(int id)
    {
        if (LoginResult is not LoginSuccessful) return null;

        players.TryGetValue(id, out var player);
        return player;
    }

    public LocationDTO? FindLocation(long id, string? game = null)
    {
        if (LoginResult is not LoginSuccessful) return null;

        game ??= _game;
        if (!locations.TryGetValue((id, game), out var location))
        {
            var locationName = ArchipelagoSession.Locations.GetLocationNameFromId(id, game);
            location = new LocationDTO(locationName, id, ItemHintState.Unknown);
            if (location is not null)
                locations.TryAdd((id, game), location);
        }
        return location;
    }

    public string? FindItem(long id, string? game = null)
    {
        if (LoginResult is not LoginSuccessful) return null;

        game ??= _game;
        if (!items.TryGetValue((id, game), out var item))
        {
            item = ArchipelagoSession.Items.GetItemName(id, game);
            if (item is not null)
                items.TryAdd((id, game), item);
        }
        return item;
    }

    public string? FindGame(int id)
    {
        if (LoginResult is not LoginSuccessful) return null;

        games.TryGetValue(id, out var game);
        return game;
    }

    public List<LocationDTO?> ListMyLocations()
    {
        if (LoginResult is not LoginSuccessful) return [];

        return ArchipelagoSession.Locations.AllLocations.Select(x => FindLocation(x)).ToList();
    }


    public async Task Connect(string url, string game, string name, string password, Version? version = null)
    {
        if (ArchipelagoSession is not null)
            await Disconnect();

        try
        {
            _url = url;
            _game = game;
            _name = name;
            _password = password;
            _version = version;
            ArchipelagoSession = ArchipelagoSessionFactory.CreateSession(url);
            DeathLinkService = ArchipelagoSession.CreateDeathLinkService();
            ArchipelagoSession.Hints.TrackHints(UpdateHints);
            this.ArchipelagoSession.MessageLog.OnMessageReceived += AddLogMessage;

            // Uses the async connect/login API (never blocks the calling thread waiting on a
            // background task) rather than the synchronous TryConnectAndLogin, which internally
            // does block - fine on a real thread pool (desktop) but deadlocks/times out on the
            // single-threaded WASM host.
            await ArchipelagoSession.ConnectAsync();
            LoginResult = await ArchipelagoSession.LoginAsync(_game, _name, ItemsHandlingFlags.AllItems, password: _password, version: _version);
            await FillData();

        }
        catch (Exception e)
        {
            LoginResult = new LoginFailure(e.GetBaseException().Message);
        }


        if (LoginResult is LoginFailure failure)
        {
            ConnectionMessage = $"Failed to connect to {_url} as {_name}:";

            foreach (string error in failure.Errors)
            {
                ConnectionMessage += $"\n    {error}";
            }
            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                ConnectionMessage += $"\n    {error}";
            }
        }
        else
        {
            ConnectionMessage = $"Connected.";
            ArchipelagoSession.Hints.TrackHints(UpdateHints);
            ItemGroups = await ArchipelagoSession.DataStorage.GetItemNameGroupsAsync();
        }
    }

    public async Task Disconnect()
    {
        if (ArchipelagoSession is not null)
        {
            try
            {
                await ArchipelagoSession.Socket.DisconnectAsync();
            }
            catch
            {
                // socket may already be closed/faulted - nothing more to do
            }
        }

        ArchipelagoSession = null;
        DeathLinkService = null;
        LoginResult = null;
        ItemGroups = null;
        ConnectionMessage = "Disconnected.";

        players.Clear();
        locations.Clear();
        items.Clear();
        games.Clear();
        hints.Clear();
    }

    private void AddLogMessage(LogMessage message)
    {
        _chatService.Add(message);
    }

    public async Task SendChatMessage(string message)
    {
        await ArchipelagoSession.SayAsync(message);
    }

    private async Task FillData()
    {
        foreach (var player in ArchipelagoSession.Players.AllPlayers)
        {
            players.TryAdd(player.Slot, player.Alias);
            games.TryAdd(player.Slot, player.Game);
        }

        var scouted = await ArchipelagoSession.Locations.ScoutLocationsAsync(ArchipelagoSession.Locations.AllLocationsChecked.ToArray());
        var items = scouted.Select(x => (x.Value.LocationName, x.Value.LocationId, x.Value.ItemName));
        foreach (var location in items)
        {
            locations.TryAdd((location.LocationId, _game), new LocationDTO(location.LocationName, location.LocationId, ItemHintState.Collected, location.ItemName));
        }

        foreach (var location in ArchipelagoSession.Locations.AllMissingLocations)
        {
            var name = ArchipelagoSession.Locations.GetLocationNameFromId(location);
            if (name is not null)
                locations.TryAdd((location, _game), new LocationDTO(name, location, ItemHintState.Unknown));
        }
    }

    private void UpdateHints(Hint[] hintlist)
    {
        hints.Clear();
        foreach (var hint in hintlist)
        {
            try
            {
                var receivingPlayer = FindPlayer(hint.ReceivingPlayer);
                var item = FindItem(hint.ItemId, FindGame(hint.ReceivingPlayer));
                var findingPlayer = FindPlayer(hint.FindingPlayer);
                var location = FindLocation(hint.LocationId, FindGame(hint.FindingPlayer));
                var entrance = hint.Entrance;
                var status = hint.Status;
                hints.Add(new HintDTO(receivingPlayer, item, findingPlayer, location?.Name, entrance, status));

                if (location is not null)
                {
                    location.Item = item;
                    switch (status)
                    {
                        case HintStatus.Found:
                            location.HintState = ItemHintState.Collected;
                            break;
                        default:
                            location.HintState = ItemHintState.Hinted;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not load a hint: {ex.Message}");
            }
        }
        HintsChanged?.Invoke(this, hints);
    }
}
