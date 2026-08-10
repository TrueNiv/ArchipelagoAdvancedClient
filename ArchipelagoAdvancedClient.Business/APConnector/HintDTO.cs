using Archipelago.MultiClient.Net.Enums;

namespace ArchipelagoAdvancedClient.Business.APConnector;

public class HintDTO
{
    public string? ReceivingPlayer {get;set;}
    public string? Item {get;set;}
    public string? FindingPlayer {get;set;}
    public string? Location {get;set;}
    public string? Entrance {get;set;}
    public HintStatus Status {get;set;}
    public bool IsReceivingPlayerSelf {get;set;}
    public bool IsFindingPlayerSelf {get;set;}

    public HintDTO(string? receivingPlayer, string? item, string? findingPlayer, string? location, string? entrance, HintStatus status, bool isReceivingPlayerSelf, bool isFindingPlayerSelf)
    {
        ReceivingPlayer = receivingPlayer;
        Item = item;
        FindingPlayer = findingPlayer;
        Location = location;
        Entrance = entrance;
        Status = status;
        IsReceivingPlayerSelf = isReceivingPlayerSelf;
        IsFindingPlayerSelf = isFindingPlayerSelf;
    }
}