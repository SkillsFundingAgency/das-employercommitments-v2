using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    [Route("error/{statuscode?}")]
    public IActionResult Error(int? statusCode)
    {
        ViewBag.ShowNav = false;

        switch (statusCode)
        {
            case 400:
            case 403:
            case 404:
                return View(statusCode.ToString());
            default:
                return View();
        }
    }
}