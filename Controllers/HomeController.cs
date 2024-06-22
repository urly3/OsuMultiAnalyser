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

    [Route("/")]
    public IActionResult Index()
    {
        return View(_lobbyService.Lobbies);
    }

    [Route("/{id}")]
    public IActionResult ViewLobby(long? id)
    {
        if (id == null)
        {
            return Redirect("/");
        }

        if (!_lobbyService.Lobbies.ContainsKey(id.Value))
        {
            return NotFound("lobby " + id.Value.ToString() + " not found.");
        }

        return View(_lobbyService.Lobbies[id.Value]);
    }

    [Route("/addlobby")]
    public IActionResult AddLobby()
    {
        if (this.Request.Method == "POST")
        {
            try
            {
                int bestOf = 0;
                int warmupCount = 0;

                long id = long.Parse(this.Request.Form["LobbyIds"]!);
                if (!string.IsNullOrWhiteSpace(this.Request.Form["BestOf"]))
                {
                    bestOf = int.Parse(this.Request.Form["BestOf"]!);
                }
                if (!string.IsNullOrWhiteSpace(this.Request.Form["WarmupCount"]))
                {
                    warmupCount = int.Parse(this.Request.Form["WarmupCount"]!);
                }

                _lobbyService.AddLobby(id, bestOf, warmupCount);
            }
            catch (Exception e)
            {
                return BadRequest("bad request\n" + e);
            }

            return Redirect("/");
        }

        return View();
    }

    [Route("/removelobby")]
    public IActionResult RemoveLobby()
    {
        if (Request.Method == "POST")
        {
            try
            {
                long id = long.Parse(this.Request.Form["LobbyIds"]!);
                _lobbyService.RemoveLobby(id);
            }
            catch (Exception e)
            {
                return BadRequest("bad request\n" + e);
            }

            return Redirect("/");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
