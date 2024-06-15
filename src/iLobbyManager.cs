namespace OsuMultiAnalyser;

public interface iLobbyManager
{
    public Dictionary<long, Lobby> Lobbies { get; set; }

    public void AddLobby(long id);
    
    public void RemoveLobby(long id);

    public void InvalidArgument(string arg);
}
