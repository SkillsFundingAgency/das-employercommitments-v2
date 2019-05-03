using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [AllowAnonymous]
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int? statusCode)
        {
            var path = "/error.html";
            
            switch (statusCode)
            {
                case 403:
                case 404:
                    path = $"/{statusCode}.html";
                    break;
            }

            return File(path, "text/html");
        }
    }
}