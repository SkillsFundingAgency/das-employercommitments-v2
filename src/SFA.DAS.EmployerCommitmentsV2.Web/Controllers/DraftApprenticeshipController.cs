using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(CommitmentOperation.AccessCohort, EmployerUserRole.OwnerOrTransactor)]
    [Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IModelMapper _modelMapper;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IAuthorizationService _authorizationService;

        public DraftApprenticeshipController(
            ICommitmentsService commitmentsService,
            ILinkGenerator linkGenerator,
            IModelMapper modelMapper, ICommitmentsApiClient commitmentsApiClient,
            IAuthorizationService authorizationService)
        {
            _commitmentsService = commitmentsService;
            _linkGenerator = linkGenerator;
            _modelMapper = modelMapper;
            _commitmentsApiClient = commitmentsApiClient;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipRequest request)
        {
            try
            {
                var model = await _modelMapper.Map<AddDraftApprenticeshipViewModel>(request);
                return View(model);
            }
            catch (CohortEmployerUpdateDeniedException)
            {
                return Redirect(_linkGenerator.CohortDetails(request.AccountHashedId, request.CohortReference));
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
            var addDraftApprenticeshipRequest = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(model);
            await _commitmentsApiClient.AddDraftApprenticeship(model.CohortId.Value, addDraftApprenticeshipRequest);

            if (_authorizationService.IsAuthorized(EmployerFeature.EnhancedApproval))
            {
                return RedirectToAction("Details", "Cohort", new { model.AccountHashedId, model.CohortReference });
            }

            return Redirect(_linkGenerator.CohortDetails(model.AccountHashedId, model.CohortReference));
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}", Name="Details")]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        public async Task<IActionResult> Details(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<IDraftApprenticeshipViewModel>(request);
            var viewName = viewModel is EditDraftApprenticeshipViewModel ? "Edit" : "View";
            return View(viewName, viewModel);
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}")]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        public async Task<IActionResult> EditDraftApprenticeship(EditDraftApprenticeshipViewModel model)
        {
            var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipRequest>(model);
            await _commitmentsService.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

            if (_authorizationService.IsAuthorized(EmployerFeature.EnhancedApproval))
            {
                return RedirectToAction("Details", "Cohort", new { model.AccountHashedId, model.CohortReference });
            }

            var reviewYourCohort = _linkGenerator.CohortDetails(model.AccountHashedId, model.CohortReference);
            return Redirect(reviewYourCohort);
        }
    }
}