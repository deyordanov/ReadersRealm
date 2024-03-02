namespace ReadersRealm.Web.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
using static Common.Constants.Constants.AreasConstants;
using static Common.Constants.Constants.ErrorConstants;

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