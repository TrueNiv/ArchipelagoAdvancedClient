using Archipelago.MultiClient.Net.MessageLog.Messages;

namespace ArchipelagoAdvancedClient.Business;

public class ChatService : IChatService
{
    private readonly List<LogMessage> _log = [];

    public IReadOnlyList<LogMessage> Log => _log;
    public event Action? LogChanged;

    public void Add(LogMessage message)
    {
        _log.Add(message);
        LogChanged?.Invoke();
    }
}
