using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved/add")]
    public class CreateCohortController : Controller
    {
        private readonly IMapper<IndexRequest, IndexViewModel> _indexViewModelMapper;
        private readonly IMapper<AssignRequest, AssignViewModel> _assignViewModelMapper;
        private readonly ILinkGenerator _linkGenerator;

        public CreateCohortController(
            IMapper<IndexRequest, IndexViewModel> indexViewModelMapper,
            IMapper<AssignRequest, AssignViewModel> assignViewModelMapper,
            ILinkGenerator linkGenerator)
        {
            _indexViewModelMapper = indexViewModelMapper;
            _assignViewModelMapper = assignViewModelMapper;
            _linkGenerator = linkGenerator;
        }

        public IActionResult Index(IndexRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }

            var viewModel = _indexViewModelMapper.Map(request);

            viewModel.OrganisationsLink = _linkGenerator.YourOrganisationsAndAgreements(request.AccountHashedId);
            viewModel.PayeSchemesLink = _linkGenerator.PayeSchemes(request.AccountHashedId);

            return View(viewModel);
        }

        [Route("select-provider")]
        public IActionResult SelectProvider(SelectProviderRequest request)
        {
            var viewModel = new SelectProviderViewModel();//todo: from mapper

            return View(viewModel);
        }

        [Route("select-provider")]
        [HttpPost]
        public IActionResult SelectProvider(SelectProviderViewModel request)
        {
            //todo:hit api

            var confirmProviderRequest = new ConfirmProviderRequest();//todo: from mapper

            return RedirectToAction("ConfirmProvider", confirmProviderRequest);
        }

        [Route("confirm-provider")]
        [HttpGet]
        public IActionResult ConfirmProvider(ConfirmProviderRequest request)
        {
            return View(request);
        }

        [Route("assign")]
        public IActionResult Assign(AssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Error");
            }

            var viewModel = _assignViewModelMapper.Map(request);

            return View(viewModel);
        }

        [Route("assign")]
        [HttpPost]
        public IActionResult Assign(AssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var routeValues = new
            {
                model.AccountHashedId,
                model.EmployerAccountLegalEntityPublicHashedId,
                model.ReservationId,
                model.StartMonthYear,
                model.CourseCode,
                model.UkPrn
            };

            switch (model.WhoIsAddingApprentices)
            {
                case WhoIsAddingApprentices.Employer:
                    return RedirectToAction("Apprentice", routeValues);
                case WhoIsAddingApprentices.Provider:
                    return RedirectToAction("Message", routeValues);
                default:
                    return RedirectToAction("Error", "Error");
            }
        }

        [Route("apprentice")]
        public IActionResult Apprentice(ApprenticeRequest request)
        {
            return View();
        }

        [Route("message")]
        public IActionResult Message(MessageRequest request)
        {
            return View();
        }
    }
}