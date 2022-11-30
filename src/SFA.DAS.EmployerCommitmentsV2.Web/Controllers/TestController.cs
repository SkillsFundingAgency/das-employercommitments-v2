using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountHashedId}/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromServices] IOuterApiClient outerApiClient)
        {
            var result = await outerApiClient.Get<ResponseRole>("Test/Call/Commitment");
            return View(result.Role);
        }
    }
    public class ResponseRole
    {
        public string Role { get; set; }
    }
}
