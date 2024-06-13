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
                case MenuOption.Second: return dontClose;
                case MenuOption.Third: return dontClose;
                case MenuOption.Zero: { dontClose = false; break; }
                default: return true;
            }
        }
        catch
        {
            InvalidArgument();
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
            InvalidArgument();
            return;
        }

        try
        {
            AddLobby(lobbyId);
        }
        catch
        {
            InvalidArgument();
        }

        return;
    }

    public void RemoveMenu()
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
            InvalidArgument();
            return;
        }

        RemoveLobby(lobbyId);

        return;
    }

    static void InvalidArgument()
    {
        Console.Clear();
        Console.WriteLine("invalid argument");
        Thread.Sleep(500);
        Console.Clear();
    }
}
