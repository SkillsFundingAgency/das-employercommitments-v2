using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error")]
        public IActionResult Error(int? statusCode)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1            
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                var OriginalURL =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
                ViewBag.HashedAccountId = OriginalURL;
            }

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