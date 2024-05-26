namespace OsuMultiAnalyser;

class Program {
    static void Main(string[] args) {
        int bestOf = 0;

        if (args.Length == 1) {
            bestOf = int.Parse(args[0]);
        }

        var lobby = Lobby.Parse(111534249, bestOf);

        lobby.Go();
    }
}
