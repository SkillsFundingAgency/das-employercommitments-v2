using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;
using System;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(CommitmentOperation.AccessCohort, EmployerUserRole.OwnerOrTransactor)]
    [Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IModelMapper _modelMapper;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        public const string ApprenticeDeletedMessage = "Apprentice record deleted";

        public DraftApprenticeshipController(
            ICommitmentsService commitmentsService,
            IModelMapper modelMapper, ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsService = commitmentsService;
            _modelMapper = modelMapper;
            _commitmentsApiClient = commitmentsApiClient;
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
                return RedirectToAction("Details", "Cohort", new { request.AccountHashedId, request.CohortReference });
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
            var addDraftApprenticeshipRequest = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(model);
            await _commitmentsApiClient.AddDraftApprenticeship(model.CohortId.Value, addDraftApprenticeshipRequest);

            return RedirectToAction("Details", "Cohort", new { model.AccountHashedId, model.CohortReference });
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

            return RedirectToAction("Details", "Cohort", new { model.AccountHashedId, model.CohortReference });
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/Delete", Name= "DeleteDraftApprenticeship")]
        public async Task<IActionResult> DeleteDraftApprenticeship(DeleteApprenticeshipRequest request)
        {
            try
            {
                var model = await _modelMapper.Map<DeleteDraftApprenticeshipViewModel>(request);
                return View(model);
            }
            catch (Exception e) when (e is CohortEmployerUpdateDeniedException || e is DraftApprenticeshipNotFoundException)
            {
                return RedirectToCohortDetails(request.AccountHashedId, request.CohortReference);
            }
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/Delete")]
        public async Task<IActionResult> DeleteDraftApprenticeship(DeleteDraftApprenticeshipViewModel model)
        {
            if (model.ConfirmDelete.Value)
            {
                var request = await _modelMapper.Map<DeleteDraftApprenticeshipRequest>(model);
                await _commitmentsApiClient.DeleteDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId.Value, request);
                TempData.AddFlashMessage(ApprenticeDeletedMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
                return await RedirectToCohortDetailsOrCohortsPage(model);
            }

            return RedirectToOriginForDelete(model.Origin, model.AccountHashedId, model.CohortReference, model.DraftApprenticeshipHashedId);
        }

        private async Task<IActionResult> RedirectToCohortDetailsOrCohortsPage(DeleteDraftApprenticeshipViewModel model)
        {
            if (await CohortExists(model.CohortId))
            {
                return RedirectToAction("Details", "Cohort", new { model.AccountHashedId, model.CohortReference });
            }
            return RedirectToAction("Cohorts", "Cohort", new { model.AccountHashedId });
        }

        private async Task<bool> CohortExists(long? cohortId)
        {
            try
            {
                await _commitmentsApiClient.GetCohort(cohortId.Value);
                return true;
            }
            catch(RestHttpClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }

        private IActionResult RedirectToOriginForDelete(DeleteDraftApprenticeshipOrigin origin,
        string accountHashedId,
        string cohortReference,
        string draftApprenticeshipHashedId)
        {
             return (origin == DeleteDraftApprenticeshipOrigin.CohortDetails) 
                ? RedirectToCohortDetails(accountHashedId, cohortReference)
                : RedirectToAction("Details", new { accountHashedId, cohortReference, draftApprenticeshipHashedId });
        }

        private IActionResult RedirectToCohortDetails(string accountHashedId, string cohortReference)
        {
            return RedirectToAction("Details", "Cohort", new { accountHashedId, cohortReference });
        }
    }
}