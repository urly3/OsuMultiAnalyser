namespace OsuMultiAnalyser.Services;

public class LobbyService
{
    public Dictionary<long, Lobby> Lobbies { get; set; } = new();

    public void AddLobby(long id, int bestOf, int warmupCount)
    {
        var lobby = Lobby.Parse(id, bestOf, warmupCount);
        lobby.Go();
        Lobbies.Add(id, lobby);
    }

    public void RemoveLobby(long id)
    {
        this.Lobbies.Remove(id);
    }

    public LobbyService()
    {
        this.AddLobby(111534249, 13, 0);
        this.AddLobby(111253985, 11, 0);
    }
}
