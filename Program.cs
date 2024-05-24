using System.Net;
using System.Runtime.ExceptionServices;
using Microsoft.VisualBasic;

namespace OsuMultiAnalyser;

class Program
{
    static void Main(string[] args)
    {
        var lobby = Lobby.Parse(111534249);
        _ = lobby.users ?? throw new Exception("no users");
        _ = lobby.events ?? throw new Exception("no events");


        foreach (var gameEvent in lobby.events.Where(e => e.game != null))
        {
            var game = gameEvent.game;
            if (game.beatmap == null || game.beatmap.beatmapset == null)
            {
                Console.WriteLine("deleted mapset");
            }
            else
            {
                Console.Write(game.beatmap.beatmapset.artist);
                Console.Write(" - ");
                Console.Write(game.beatmap.beatmapset.title);
                Console.Write("\n");
            }

            foreach (var user in lobby.users)
            {
                var avgScore = user.CaculateAverageScore(game.scores);
                if (avgScore == null)
                {
                    continue;
                }

                Console.WriteLine(user.username);
                Console.Write("avg score: ");
                Console.WriteLine(avgScore);
            }

            Console.WriteLine();

        }

        lobby.GetWinningTeam();

        Console.Write("winning team: ");
        Console.WriteLine(lobby.WinningTeam);

        Console.Write("score: ");
        Console.Write(lobby.BlueWins.ToString());
        Console.Write(" - ");
        Console.WriteLine(lobby.RedWins.ToString());
    }
}
