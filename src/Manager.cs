namespace OsuMultiAnalyser;

public class Manager
{
    enum MenuOption
    {
        Zero = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
    }

    public Dictionary<long, Lobby> Lobbies { get; set; } = new();

    public void AddLobby(long id)
    {
        try
        {
            Lobbies.Add(id, Lobby.Parse(id));
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

    public void Run()
    {
        Menu();
    }

    public void Close()
    {

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
            Console.WriteLine("0 - exit menu");

            string input = Console.ReadKey(true).KeyChar.ToString();
            int optionInt = int.Parse(input);

            MenuOption option = (MenuOption)optionInt;

            switch (option)
            {
                case MenuOption.First: { AddMenu(); break; }
                case MenuOption.Second: { RemoveMenu(); break; }
                case MenuOption.Third: return dontClose;
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
            lobbyId = Int64.Parse(input!);
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
            lobbyId = Int64.Parse(input!);
        }
        catch
        {
            InvalidArgument("non-integer.");
            return;
        }

        RemoveLobby(lobbyId);

        return;
    }

    static void InvalidArgument(string arg)
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
