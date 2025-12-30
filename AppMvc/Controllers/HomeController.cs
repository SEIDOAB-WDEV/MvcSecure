using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppMvc.Models;
using Services;

namespace AppMvc.Controllers;

public class HomeController : Controller
{
    readonly ILogger<HomeController> _logger;
    readonly IAdminService _service = null;

    public HomeController(ILogger<HomeController> logger, IAdminService service)
    {
        _logger = logger;
        _service = service; 
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> DataSourceInfo()
    {
        var vm = new DataSourceInfoViewModel(){Info = (await _service.GuestInfoAsync()).Item};
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

