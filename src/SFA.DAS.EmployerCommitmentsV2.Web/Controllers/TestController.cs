using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountHashedId}/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index([FromServices] IOuterApiClient outerApiClient)
        {
            var result = outerApiClient.Get<ResponseRole>("Test/Call/Commitment");
            return Ok(result);
        }
    }
    public class ResponseRole
    {
        public string Role { get; set; }
    }
}
