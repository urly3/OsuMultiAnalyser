namespace OsuMultiAnalyser.Services;

public class LobbyService
{
    public Dictionary<long, Lobby> Lobbies { get; set; } = new();

    public void AddLobby(long id)
    {
        var lobby = Lobby.Parse(id);
        lobby.Go();

        Lobbies.Add(id, lobby);
    }

    public void RemoveLobby(long id) { }

    public LobbyService()
    {
        var lobby = Lobby.Parse(111534249);
        lobby.Go();
        Lobbies.Add(111534249, lobby);
    }
}
