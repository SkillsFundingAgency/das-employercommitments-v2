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
        public const string ApprenticeDeletedMessage = "Apprentice record deleted";

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

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/Delete/{Origin}", Name= "DeleteDraftApprenticeship")]
        [DasAuthorize(EmployerFeature.EnhancedApproval)]
        public async Task<IActionResult> DeleteDraftApprenticeship(DeleteApprenticeshipRequest request)
        {
            try
            {
                var model = await _modelMapper.Map<DeleteDraftApprenticeshipViewModel>(request);
                return View(model);
            }
            catch (CohortEmployerUpdateDeniedException)
            {
                return RedirectToOriginForDelete(request.Origin, request.AccountHashedId, request.CohortReference, request.DraftApprenticeshipHashedId);
            }
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/Delete/{Origin}")]
        [DasAuthorize(EmployerFeature.EnhancedApproval)]
        public async Task<IActionResult> DeleteDraftApprenticeship(DeleteDraftApprenticeshipViewModel model)
        {
            if (model.ConfirmDelete.Value)
            {
                var request = await _modelMapper.Map<DeleteDraftApprenticeshipRequest>(model);
                await _commitmentsApiClient.DeleteDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId.Value, request);
                TempData.AddFlashMessage(ApprenticeDeletedMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
                return RedirectToAction("Details", "Cohort", new {model.AccountHashedId, model.CohortReference});
            }

            return RedirectToOriginForDelete(model.Origin, model.AccountHashedId, model.CohortReference, model.DraftApprenticeshipHashedId);
        }

        private IActionResult RedirectToOriginForDelete(Origin origin,
        string accountHashedId,
        string cohortReference,
        string draftApprenticeshipHashedId)
        {
            return (origin == Origin.CohortDetails)
                 ? RedirectToAction("Details", "Cohort", new { accountHashedId, cohortReference })
                 : RedirectToAction("Details", new { accountHashedId, cohortReference, draftApprenticeshipHashedId });
        }

}
}