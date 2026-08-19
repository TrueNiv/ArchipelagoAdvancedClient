using Archipelago.MultiClient.Net.MessageLog.Messages;

namespace ArchipelagoAdvancedClient.Business;

public class ChatService : IChatService
{
    private readonly List<LogMessage> _log = [];
    private readonly object _gate = new();

    // Archipelago.MultiClient.Net raises MessageLog.OnMessageReceived on its own socket-receive
    // thread, so Add() below runs concurrently with UI-thread renders reading Log - snapshotting
    // under a lock keeps _log itself from ever being enumerated while it's being mutated.
    public IReadOnlyList<LogMessage> Log
    {
        get
        {
            lock (_gate)
            {
                return _log.ToArray();
            }
        }
    }

    public event Action? LogChanged;

    public void Add(LogMessage message)
    {
        lock (_gate)
        {
            _log.Add(message);
        }

        LogChanged?.Invoke();
    }
}
