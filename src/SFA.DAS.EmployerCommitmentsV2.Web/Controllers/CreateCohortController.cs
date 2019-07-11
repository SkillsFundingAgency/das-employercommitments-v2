using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.Owner)]
    [Route("{accountId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        public IActionResult Index(StartRequest startRequest)
        {
            return View(startRequest);
        }

        [Route("select-provider")]
        public IActionResult SelectProvider(SelectProviderRequest request)
        {
            var vm = new SelectProviderViewModel
            {
                AccountId = request.AccountId,
                CourseCode =  request.CourseCode,
                EmployerAccountLegalEntityPublicHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ReservationId = request.ReservationId,
                StartMonthYear = request.StartMonthYear
            };

            return View(vm);
        }

        [Route("select-provider")]
        [HttpPost]
        public IActionResult SelectProvider(SelectProviderViewModel request)
        {
            //hit api

            var r = new ConfirmProviderRequest
            {
                AccountId = request.AccountId,
                CourseCode = request.CourseCode,
                EmployerAccountLegalEntityPublicHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ProviderId = request.ProviderId,
                ReservationId = request.ReservationId,
                StartMonthYear = request.StartMonthYear,
            };

            return RedirectToAction("ConfirmProvider", r);
        }


        [Route("confirm-provider")]
        [HttpGet]
        public IActionResult ConfirmProvider(ConfirmProviderRequest request)
        {
            return View(request);
        }


    }
}