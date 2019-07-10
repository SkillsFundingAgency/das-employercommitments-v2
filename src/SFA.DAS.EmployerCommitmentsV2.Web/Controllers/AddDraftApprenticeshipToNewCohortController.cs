using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountId}/unapproved/add")]
    public class AddDraftApprenticeshipToNewCohortController : Controller
    {
        public IActionResult Index(IndexViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}