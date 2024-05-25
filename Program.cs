namespace OsuMultiAnalyser;

class Program {
    static void Main(string[] args) {
        var lobby = Lobby.Parse(111534249);
        _ = lobby.users ?? throw new Exception("no users");
        _ = lobby.events ?? throw new Exception("no events");


        foreach (var gameEvent in lobby.CompletedGames) {
            var game = gameEvent.game;
            if (game?.beatmap == null || game.beatmap.beatmapset == null) {
                Console.WriteLine("deleted mapset");
            } else {
                Console.Write((string?)game.beatmap.beatmapset.artist);
                Console.Write(" - ");
                Console.Write((string?)game.beatmap.beatmapset.title);
                Console.Write('\n');
            }
        }

        Console.Write("\n");

        lobby.CalculateStats();

        Console.Write("winning team: ");
        Console.WriteLine(lobby.WinningTeam);

        Console.Write("score: ");
        Console.Write(lobby.BlueWins.ToString() + " - " + lobby.RedWins.ToString() + "\n\n");

        Console.WriteLine("average player stats: ");

        foreach (var user in lobby.users)
        {
            Console.WriteLine(user.username);
            Console.WriteLine("avg score:\t" + user.AverageScore.ToString("n0"));
            Console.WriteLine("avg acc:\t" + user.AverageAccuracy.ToString("f2") + '%');
            Console.WriteLine("maps played:\t" + user.MapsPlayed.ToString());
            Console.Write('\n');
        }

        Console.Write('\n');

        Console.WriteLine("highest avg score: ");
        Console.WriteLine(lobby.users.First(u => u.id == lobby.HighestAverageScore.Item1).username);
        Console.WriteLine(lobby.HighestAverageScore.Item2);

        Console.Write('\n');

        Console.WriteLine("highest avg accuracy: ");
        Console.WriteLine(lobby.users.First(u => u.id == lobby.HighestAverageAccuracy.Item1).username);
        Console.WriteLine(lobby.HighestAverageAccuracy.Item2);
    }
}
