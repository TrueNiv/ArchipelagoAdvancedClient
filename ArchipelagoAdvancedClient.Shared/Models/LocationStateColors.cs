using ArchipelagoAdvancedClient.Business.APConnector;

namespace ArchipelagoAdvancedClient.Shared.Models;

public static class LocationStateColors
{
    public const string Unknown = "#e7e7e7";
    public const string Peeked = "#00BFFF";
    public const string Hinted = "#FFA500";
    public const string Collected = "#00FF7F";

    public static string ForState(ItemHintState state) => state switch
    {
        ItemHintState.Peeked => Peeked,
        ItemHintState.Hinted => Hinted,
        ItemHintState.Collected => Collected,
        _ => Unknown,
    };
}
