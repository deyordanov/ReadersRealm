namespace ReadersRealm.Web.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
using static Common.Constants.Constants.Areas;
using static Common.Constants.Constants.Error;

[Area(Customer)]
public class ErrorController : BaseController
{
    [HttpGet]
    [Route(NotFound404Path)]
    public IActionResult NotFound404()
    {
        return View();
    }
}