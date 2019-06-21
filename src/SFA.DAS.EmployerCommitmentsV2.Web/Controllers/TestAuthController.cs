using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Authorize]
    [Route("test")]
    public class TestAuthController : Controller
    {
        [Route("account/{AccountHashedId}/cohort/{CohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public IActionResult AccessTest(TestRequest request)
        {
            return View("Index");
        }

        [Route("account/{AccountHashedId}")]
        [DasAuthorize(EmployerUserRole.Owner)]
        public IActionResult RoleTest(TestRequest request)
        {
            return View("Index");
        }

        [Route("noaccount")]
        public IActionResult UnauthenticatedTest()
        {
            return View("Index");
        }


    }
}