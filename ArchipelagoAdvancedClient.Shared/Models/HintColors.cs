using Archipelago.MultiClient.Net.Enums;

namespace ArchipelagoAdvancedClient.Shared.Models;

public static class HintColors
{
    public const string PlayerSelf = "#990BB1";
    public const string PlayerOther = "#e7e7e7";
    public const string Location = "#00FF7F";
    public const string EntranceVanilla = "#4375c2";
    public const string EntranceShuffled = "#355e9e";

    public const string StatusFound = "#00FF7F";
    public const string StatusUnspecified = "#e7e7e7";
    public const string StatusNoPriority = "#00EEEE";
    public const string StatusAvoid = "#FA8072";
    public const string StatusPriority = "#57549d";

    public static string ForStatus(HintStatus status) => status switch
    {
        HintStatus.Found => StatusFound,
        HintStatus.NoPriority => StatusNoPriority,
        HintStatus.Avoid => StatusAvoid,
        HintStatus.Priority => StatusPriority,
        _ => StatusUnspecified,
    };
}
