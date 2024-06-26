using OMA.Models;
using System.Data.HashFunction.FNV;
using System.Text;

namespace OMA.Services;

public class OMAService
{
    public static Dictionary<string, Alias> UserLobbies { get; set; } = new();
    private readonly IFNV1a _fnv;
    private readonly string _kaneHash;

    public void AddLobby(string hash, long id, int bestOf, int warmupCount)
    {
        var alias = UserLobbies.Values.Where(a => a.Lobbies.ContainsKey(id)).FirstOrDefault();
        if (alias != null)
        {
            UserLobbies[hash].Lobbies.Add(id, alias.Lobbies[id]);
            return;
        }

        var lobby = Lobby.Parse(id, bestOf, warmupCount);
        lobby.Go();
        UserLobbies[hash].Lobbies.Add(id, lobby);
    }

    public void RemoveLobby(string hash, long id)
    {
        UserLobbies[hash].Lobbies.Remove(id);
    }

    public string AddUser(string alias)
    {
        var hash = _fnv.ComputeHash(Encoding.UTF8.GetBytes(alias)).AsBase64String();
        if (UserLobbies.ContainsKey(hash))
        {
            if (hash == _kaneHash)
            {
                return _kaneHash;
            }
            throw new Exception("user already exists");
        }

        UserLobbies.Add(hash, new());
        return hash;
    }

    public static bool ValidateHash(string hash)
    {
        return UserLobbies.ContainsKey(hash);
    }

    public void AliasSwapLock(string hash)
    {
        if (!ValidateHash(hash))
        {
            return;
        }

        Alias alias = UserLobbies[hash];
        if (alias.IsLocked())
        {
            // alias.Unlock();
        }
        else
        {
            alias.Lock();
        }
    }

    public void UnlockAlias(string hash)
    {
        UserLobbies[hash].Unlock();
    }

    public bool AliasLocked(string hash)
    {
        return string.IsNullOrWhiteSpace(hash)
            || UserLobbies[hash].IsLocked();
    }

    public OMAService()
    {
        _fnv = FNV1aFactory.Instance.Create();
        _kaneHash = AddUser("kane");

        AddLobby(_kaneHash, 111534249, 13, 0);
        AddLobby(_kaneHash, 111253985, 11, 0);
    }
}
