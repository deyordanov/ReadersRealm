namespace ReadersRealm.Web.Areas;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[AutoValidateAntiforgeryToken]
public class BaseController : Controller
{
    
}