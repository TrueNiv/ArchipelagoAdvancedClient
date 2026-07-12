using Archipelago.MultiClient.Net.MessageLog.Messages;

namespace ArchipelagoAdvancedClient.Business;

public interface IChatService
{
    IReadOnlyList<LogMessage> Log { get; }
    event Action? LogChanged;
    void Add(LogMessage message);
}
