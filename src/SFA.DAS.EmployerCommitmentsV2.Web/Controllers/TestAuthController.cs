using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("test")]
    public class TestAuthController : Controller
    {
        [Route("account/{AccountHashedId}/cohort/{CohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public IActionResult Index(TestRequest request)
        {
            return View();
        }

        [Route("account/{AccountHashedId}")]
        [DasAuthorize(EmployerUserRole.Any)]
        public IActionResult Index2(TestRequest request)
        {
            return View("Index");
        }

        [Route("noaccount")]
        public IActionResult Index3()
        {
            return View("Index");
        }


    }
}