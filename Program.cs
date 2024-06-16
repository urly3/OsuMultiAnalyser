namespace OsuMultiAnalyser;

class Program
{
    static void Main(string[] args)
    {
        int bestOf = 0;
        int warmups = 0;


        if (args.Length > 0)
        {
            bestOf = int.Parse(args[0]);


            if (args.Length > 1)
            {
                warmups = int.Parse(args[1]);
            }
        }

        var manager = new MvcLobbyManager(new string[0]);
    }
}
