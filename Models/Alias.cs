namespace OMA.Models;

public class Alias
{
    private bool _locked = false;
    public Dictionary<long, Lobby> Lobbies = new();

    public bool IsLocked()
    {
        return _locked;
    }

    public void Lock()
    {
        _locked = true;
    }
    public void Unlock()
    {
        _locked = false;
    }
}
