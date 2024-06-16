using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OsuMultiAnalyser.Models;
using OsuMultiAnalyser.Services;

namespace OsuMultiAnalyser;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private LobbyService _lobbyService;

    public HomeController(ILogger<HomeController> logger, LobbyService lobbyService)
    {
        _logger = logger;
        _lobbyService = lobbyService;
    }

    public IActionResult Index()
    {
        return View(_lobbyService.Lobbies);
    }

    public IActionResult AddLobby()
    {
        if (this.Request.Method == "POST")
        {
            long id = long.Parse(this.Request.Form["LobbyIds"]!);
            _lobbyService.AddLobby(id);
            return Redirect("/");
        }

        return View();
    }

    public IActionResult RemoveLobby()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
