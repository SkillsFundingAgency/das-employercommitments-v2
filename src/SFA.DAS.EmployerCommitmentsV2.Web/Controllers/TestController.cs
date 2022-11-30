using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index([FromServices] IOuterApiClient outerApiClient)
        {
            var result = outerApiClient.Get<string>("Test/Call/Commitment");
            return Ok(result);
        }
    }
}
