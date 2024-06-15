namespace OsuMultiAnalyser;

public interface iLobbyManager
{
    public Dictionary<long, Lobby> Lobbies { get; set; } = new();

    public void AddLobby(long id)
    {
        try
        {
            var lobby = Lobby.Parse(id);
            lobby.Go();
            this.Lobbies.Add(id, Lobby.Parse(id));
        }
        catch (Exception e)
        {
            throw new Exception("invalid id or lobby", e);
        }
    }

    public void RemoveLobby(long id)
    {
        if(!this.Lobbies.Remove(id))
        {
            throw new Exception("lobby does not exist");
        }
    }

    public void Run();

    public void Close();

    static void InvalidArgument(string arg);
}
