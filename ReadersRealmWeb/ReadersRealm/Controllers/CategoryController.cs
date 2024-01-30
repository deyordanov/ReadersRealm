using Microsoft.AspNetCore.Mvc;

namespace ReadersRealm.Controllers;

public class CategoryController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}