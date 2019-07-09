using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountId}/unapproved/add")]
    public class AddDraftApprenticeshipToNewCohortController : Controller
    {
        public IActionResult Index(string accountId)
        {
            return View(accountId);
        }
    }
}