using OMA.Models;
using System.Data.HashFunction.FNV;
using System.Text;

namespace OMA.Services;

public class OMAService
{
    public static Dictionary<string, Dictionary<long, Lobby>> UserLobbies { get; set; } = new();
    private readonly IFNV1a _fnv;
    private readonly string _kaneHash;

    public void AddLobby(string hash, long id, int bestOf, int warmupCount)
    {
        var match = UserLobbies.Values.Where(d => d.ContainsKey(id)).FirstOrDefault();
        if (match != null)
        {
            UserLobbies[hash].Add(id, match[id]);
            return;
        }

        var lobby = Lobby.Parse(id, bestOf, warmupCount);
        lobby.Go();
        UserLobbies[hash].Add(id, lobby);
    }

    public void RemoveLobby(string hash, long id)
    {
        UserLobbies[hash].Remove(id);
    }

    public string AddUser(string alias)
    {
        var hash = _fnv.ComputeHash(Encoding.UTF8.GetBytes(alias)).AsBase64String();
        if (UserLobbies.ContainsKey(hash))
        {
            if (hash  == _kaneHash)
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

    public OMAService()
    {
        _fnv = FNV1aFactory.Instance.Create();
        _kaneHash = AddUser("kane");

        AddLobby(_kaneHash, 111534249, 13, 0);
        AddLobby(_kaneHash, 111253985, 11, 0);
    }
}
