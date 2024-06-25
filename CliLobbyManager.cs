using OMA.Models;

namespace OMA;

class CliLobbyManager
{
    public Dictionary<long, Lobby> Lobbies { get; set; } = new();

    enum MenuOption
    {
        Zero = 0,
        First = 1,
        Second = 2,
        Third = 3,
    }

    public CliLobbyManager()
    {
        Menu();
    }

    public void AddLobby(long id)
    {
        try
        {
            Lobbies.Add(id, Lobby.Parse(id));
            Lobbies.GetValueOrDefault(id)?.Go();
        }
        catch
        {
            throw new Exception("invalid id or lobby");
        }
    }

    public void RemoveLobby(long id)
    {
        Lobbies.Remove(id);
    }

    public void ViewLobby(long id)
    {
        Lobby lobby = this.Lobbies.GetValueOrDefault(id)!;
        Console.WriteLine("score: ");
        Console.WriteLine(lobby.BlueWins.ToString() + " - " + lobby.RedWins.ToString());
        Console.WriteLine(lobby.WinningTeam + " wins");
        Console.WriteLine("averages: ");
        Console.WriteLine("highest average score: " +
            lobby.users?.Where(u => u.id == lobby.HighestAverageScore.Item1).First().username +
            " - " +
            lobby.HighestAverageScore.Item2.ToString());
        Console.WriteLine("highest average acc: " +
            lobby.users?.Where(u => u.id == lobby.HighestAverageAccuracy.Item1).First().username +
            " - " +
            lobby.HighestAverageAccuracy.Item2.ToString());

        Console.WriteLine();
        Console.WriteLine("press q to exit");

        while (Console.ReadKey(true).KeyChar != 'q') { }
    }

    public void Menu()
    {
        while (MainMenu()) { }
    }

    public bool MainMenu()
    {
        bool dontClose = true;

        try
        {
            Console.Clear();
            Console.WriteLine("please choose an option: ");
            Console.WriteLine("1 - add a lobby");
            Console.WriteLine("2 - remove a lobby");
            Console.WriteLine("3 - view a lobby");
            Console.WriteLine("q - exit menu");

            char input = Console.ReadKey(true).KeyChar;

            MenuOption option;

            if (input == 'q')
            {
                option = MenuOption.Zero;
            }
            else
            {
                int optionInt = int.Parse(input.ToString());
                option = Enum.Parse<MenuOption>(optionInt.ToString());
            }

            switch (option)
            {
                case MenuOption.First: { AddMenu(); break; }
                case MenuOption.Second: { RemoveMenu(); break; }
                case MenuOption.Third: { ViewMenu(); break; }
                case MenuOption.Zero: { dontClose = false; break; }
                default: { throw new Exception(); }
            }
        }
        catch
        {
            InvalidArgument("invalid option");
        }

        return dontClose;
    }

    public void AddMenu()
    {
        Console.Clear();
        long lobbyId;

        Console.Write("enter lobby id: ");
        var input = Console.ReadLine();

        try
        {
            lobbyId = long.Parse(input!);
        }
        catch
        {
            InvalidArgument("non-integer.");
            return;
        }

        try
        {
            AddLobby(lobbyId);
        }
        catch
        {
            InvalidArgument("invalid lobby id");
        }

        return;
    }

    public void RemoveMenu()
    {
        Console.Clear();
        long lobbyId;

        PrintLobbies();

        Console.Write("enter lobby id: ");
        var input = Console.ReadLine();

        try
        {
            lobbyId = long.Parse(input!);
        }
        catch
        {
            InvalidArgument("non-integer.");
            return;
        }

        RemoveLobby(lobbyId);

        return;
    }

    public void ViewMenu()
    {
        Console.Clear();
        long lobbyId;

        PrintLobbies();

        Console.Write("enter lobby id: ");
        var input = Console.ReadLine();

        try
        {
            lobbyId = long.Parse(input!);
        }
        catch
        {
            InvalidArgument("non-integer.");
            return;
        }

        ViewLobby(lobbyId);

        return;
    }

    public void InvalidArgument(string arg)
    {
        Console.Clear();
        Console.WriteLine(arg);
        Thread.Sleep(500);
        Console.Clear();
    }

    public void PrintLobbies()
    {
        int i = 1;
        foreach (var lobby in this.Lobbies)
        {
            Console.Write(i.ToString() + ".\tid: ");
            Console.WriteLine(lobby.Key);
            Console.WriteLine();
        }
    }

}
