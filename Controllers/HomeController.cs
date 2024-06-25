using Microsoft.AspNetCore.Mvc;
using OMA.Models;
using OMA.Services;
using System.Diagnostics;

namespace OMA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private OMAService _omaService;
    private string _hash = "";

    public HomeController(ILogger<HomeController> logger, OMAService lobbyService)
    {
        _logger = logger;
        _omaService = lobbyService;
    }

    [Route("/")]
    public IActionResult Index()
    {
        _hash = Request.Cookies["hash"]!;
        return View(OMAService.UserLobbies[_hash]);
    }


    [Route("/login")]
    public IActionResult Login()
    {
        if (Request.Method == "POST")
        {
            if (string.IsNullOrWhiteSpace(Request.Form["alias"]))
            {
                return Error();
            }

            try
            {
                string hash = _omaService.AddUser(Request.Form["alias"]!);
                Response.Cookies.Append("hash", hash);

            }
            catch
            {
            }

            return Redirect("/");
        }

        _hash = Request.Cookies["hash"] ?? "";
        if (!string.IsNullOrWhiteSpace(_hash))
        {
            if (OMAService.ValidateHash(_hash))
            {
                return Redirect("/");
            }
        }

        return View();
    }

    [Route("/{id}")]
    public IActionResult ViewLobby(long? id)
    {
        _hash = Request.Cookies["hash"]!;

        if (id == null)
        {
            return Redirect("/");
        }

        if (!OMAService.UserLobbies[_hash].Lobbies.ContainsKey(id.Value))
        {
            return NotFound("lobby " + id.Value.ToString() + " not found.");
        }

        return View(OMAService.UserLobbies[_hash].Lobbies[id.Value]);
    }

    [Route("/addlobby")]
    public IActionResult AddLobby()
    {
        _hash = Request.Cookies["hash"]!;
        if (!OMAService.ValidateHash(_hash) || _omaService.AliasLocked(_hash))
        {
            return Redirect("/");
        }

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

                _omaService.AddLobby(_hash, id, bestOf, warmupCount);
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
        _hash = Request.Cookies["hash"]!;

        if (!OMAService.ValidateHash(_hash) || _omaService.AliasLocked(_hash))
        {
            return Redirect("/");
        }

        if (Request.Method == "POST")
        {
            try
            {
                long id = long.Parse(this.Request.Form["LobbyIds"]!);
                _omaService.RemoveLobby(_hash, id);
            }
            catch (Exception e)
            {
                return BadRequest("bad request\n" + e);
            }

            return Redirect("/");
        }

        return View();
    }

    [Route("/aliasswaplock")]
    public IActionResult AliasSwapLock()
    {
        _hash = Request.Cookies["hash"] ?? "";
        if (string.IsNullOrEmpty(_hash))
        {
            return Redirect("/");
        }

        _omaService.AliasSwapLock(_hash);

        return Redirect("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
