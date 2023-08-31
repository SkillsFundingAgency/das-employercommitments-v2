using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("error/{statuscode?}")]
        public IActionResult Error(int? statusCode)
        {
            ViewBag.HideNav = true;

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
}