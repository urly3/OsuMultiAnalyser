using System.Net;
using System.Runtime.ExceptionServices;

namespace OsuMultiAnalyser;

class Program
{
    static void Main(string[] args)
    {
        var lobby = Lobby.Parse(111534249);
        _ = lobby.users ?? throw new Exception("no users");

        foreach (var user in lobby.users)
        {
            Console.WriteLine(user.username);
        }
    }
}
