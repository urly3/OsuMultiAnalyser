namespace OsuMultiAnalyser;

public class Manager
{
    enum MenuOption
    {
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

    public bool AddLobbyMenu()
    {
        bool shouldClose = false;
        return shouldClose ? false : true;
    }

    public void RemoveLobby(long id)
    {
        Lobbies.Remove(id);
    }

    public bool RemoveLobbyMenu()
    {
        bool shouldClose = false;
        return shouldClose ? false : true;
    }

    public void Menu()
    {
        while (MainMenu()) { }
    }

    public bool MainMenu()
    {
        bool shouldClose = false;

        try
        {
            Console.WriteLine("please choose an option: ");
            Console.WriteLine("1 - add a lobby");
            Console.WriteLine("2 - remove a lobby");
            Console.WriteLine("3 - view a lobby");

            string input = Console.ReadKey().KeyChar.ToString();
            int optionInt = int.Parse(input);

            MenuOption option = (MenuOption)optionInt;

            switch (option)
            {
                case MenuOption.First: return false;
                case MenuOption.Second: return false;
                case MenuOption.Third: return false;
                default: return true;
            }
        }
        catch
        {
            Console.WriteLine("invalid argument");
        }

        return shouldClose ? false : true;
    }
}
