using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("badrequest")]
        public IActionResult BadRequest()
        {
            ViewBag.HideNav = true;
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return View();
        }
        
        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            ViewBag.HideNav = true;
            Response.StatusCode = (int)HttpStatusCode.Forbidden;

            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            ViewBag.HideNav = true;
            
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return View();
        }

        [Route("notfound")]
        public IActionResult NotFound()
        {
            ViewBag.HideNav = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            return View();
        }
    }
}