using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;
using SFA.DAS.Http;
using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(CommitmentOperation.AccessCohort, EmployerUserRole.OwnerOrTransactor)]
    [Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly IModelMapper _modelMapper;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IAuthorizationService _authorizationService;
        private readonly IEncodingService _encodingService;

        public const string ApprenticeDeletedMessage = "Apprentice record deleted";

        public DraftApprenticeshipController(
            IModelMapper modelMapper,
			ICommitmentsApiClient commitmentsApiClient,
            IAuthorizationService authorizationService,
            IEncodingService encodingService)
        {
            _modelMapper = modelMapper;
            _commitmentsApiClient = commitmentsApiClient;
            _authorizationService = authorizationService;
            _encodingService = encodingService;
        }

        [HttpGet]
        [Route("add")]
        public IActionResult AddNewDraftApprenticeship(AddDraftApprenticeshipRequest request)
        {
            return RedirectToAction(nameof(SelectCourse), request.CloneBaseValues());
        }

        [HttpGet]
        [Route("add/select-course")]
        public async Task<IActionResult> SelectCourse(AddDraftApprenticeshipRequest request)
        {
            var selectCourseViewModel = await _modelMapper.Map<SelectCourseViewModel>(request);
            return View("SelectCourse", selectCourseViewModel);
        }

        [HttpPost]
        [Route("add/select-course")]
        public async Task<IActionResult> SelectCourse(SelectCourseViewModel model)
        {
            if (string.IsNullOrEmpty(model.CourseCode))
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail(nameof(model.CourseCode), "You must select a training course")});
            }

            var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
            return RedirectToAction(nameof(SelectDeliveryModel), request.CloneBaseValues());
        }

        [HttpGet]
        [Route("add/select-delivery-model")]
        public async Task<IActionResult> SelectDeliveryModel(AddDraftApprenticeshipRequest request)
        {
            var model = await _modelMapper.Map<SelectDeliveryModelViewModel>(request);

            if (model.DeliveryModels.Length > 1)
            {
                return View("SelectDeliveryModel", model);
            }

            request.DeliveryModel = model.DeliveryModels.FirstOrDefault();
            return RedirectToAction(nameof(AddDraftApprenticeshipDetails), request.CloneBaseValues());
        }

        [HttpPost]
        [Route("add/select-delivery-model")]
        public async Task<IActionResult> SetDeliveryModel(SelectDeliveryModelViewModel model)
        {
            if (model.DeliveryModel == null)
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail("DeliveryModel", "You must select the apprenticeship delivery model")});
            }

            var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
            return RedirectToAction(nameof(AddDraftApprenticeshipDetails), request.CloneBaseValues());
        }

        [HttpGet]
        [Route("add-another")]
        public async Task<IActionResult> AddDraftApprenticeshipDetails(AddDraftApprenticeshipRequest request)
        {
            try
            {
                var model = GetStoredDraftApprenticeshipState();
                if (model == null)
                {
                    model = await _modelMapper.Map<AddDraftApprenticeshipViewModel>(request);
                }
                else
                {
                    model.CourseCode = request.CourseCode;
                    model.DeliveryModel = request.DeliveryModel;
                }
                
                return View("AddDraftApprenticeship", model);
            }
            catch (CohortEmployerUpdateDeniedException)
            {
                return RedirectToAction("Details", "Cohort", new { request.AccountHashedId, request.CohortReference });
            }
        }

        [HttpPost]
        [Route("add-another")]
        public async Task<IActionResult> AddDraftApprenticeshipDetails(string changeCourse, string changeDeliveryModel, AddDraftApprenticeshipViewModel model)
        {
            if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
            {
                StoreDraftApprenticeshipState(model);
                var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
                return RedirectToAction(changeCourse == "Edit" ? nameof(SelectCourse) : nameof(SelectDeliveryModel), request.CloneBaseValues());
            }

            var addDraftApprenticeshipRequest = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(model);

            var response = await _commitmentsApiClient.AddDraftApprenticeship(model.CohortId.Value, addDraftApprenticeshipRequest);

            var draftApprenticeshipHashedId = _encodingService.Encode(response.DraftApprenticeshipId, EncodingType.ApprenticeshipId);
            
            return RedirectToAction("SelectOption", "DraftApprenticeship", new { model.AccountHashedId, model.CohortReference, draftApprenticeshipHashedId });
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}", Name="Details")]
        [Route("{DraftApprenticeshipHashedId}/edit", Name="Details-Edit")]
        public async Task<IActionResult> Details(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<IDraftApprenticeshipViewModel>(request);
            var viewName = viewModel is EditDraftApprenticeshipViewModel ? "Edit" : "View";
            return View(viewName, viewModel);
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/view", Name = "Details-View")]
        public async Task<IActionResult> ViewDetails(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<ViewDraftApprenticeshipViewModel>(request);
            var viewName = "View";
            return View(viewName, viewModel);
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/edit-display", Name="EditDraftApprenticeshipDisplay")]
        public IActionResult EditDraftApprenticeshipDisplay(EditDraftApprenticeshipViewModel model)
        {
            var localModel = GetStoredEditDraftApprenticeshipState();

            if (localModel != null)
            {
                if (localModel.DeliveryModel != model.DeliveryModel)
                {
                    localModel.HasChangedDeliveryModel = true;
                }

                localModel.CourseCode = model.CourseCode;
                localModel.DeliveryModel = model.DeliveryModel;
                return View("Edit", localModel);
            }

            return View("Edit", model);
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}")]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        [Route("{DraftApprenticeshipHashedId}/edit-display")]
        public async Task<IActionResult> EditDraftApprenticeship(string changeCourse, string changeDeliveryModel, EditDraftApprenticeshipViewModel model)
        {
            if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
            {
                StoreEditDraftApprenticeshipState(model);
                var req = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
                return RedirectToAction(changeCourse == "Edit" ? nameof(SelectCourseForEdit) : nameof(SelectDeliveryModelForEdit), req.CloneBaseValues());
            }

            var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipRequest>(model);

            await _commitmentsApiClient.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

            return RedirectToAction("SelectOption", "DraftApprenticeship", new { model.AccountHashedId, model.CohortReference, model.DraftApprenticeshipHashedId });
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/edit/select-course")]
        public async Task<IActionResult> SelectCourseForEdit(AddDraftApprenticeshipRequest request)
        {
            var selectCourseViewModel = await _modelMapper.Map<SelectCourseViewModel>(request);
            return View("SelectCourse", selectCourseViewModel);
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/edit/select-course")]
        public async Task<ActionResult> SetCourseForEdit(SelectCourseViewModel model)
        {
            if (string.IsNullOrEmpty(model.CourseCode))
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail(nameof(model.CourseCode), "You must select a training course")});
            }

            var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
            return RedirectToAction(nameof(SelectDeliveryModelForEdit), request.CloneBaseValues());
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/edit/select-delivery-model")]
        public async Task<IActionResult> SelectDeliveryModelForEdit(EditDraftApprenticeshipViewModel request)
        {
            var model = await _modelMapper.Map<SelectDeliveryModelForEditViewModel>(request);

            if (model != null)
            {
                model.DeliveryModel = (EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel?) request.DeliveryModel;

                if (model.DeliveryModels.Count > 1 || model.HasUnavailableFlexiJobAgencyDeliveryModel)
                {
                    return View(model);
                }

                request.DeliveryModel = (SFA.DAS.CommitmentsV2.Types.DeliveryModel) model.DeliveryModels.FirstOrDefault();
            }

            return RedirectToAction(nameof(EditDraftApprenticeshipDisplay), request);
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/edit/select-delivery-model")]
        public async Task<IActionResult> SetDeliveryModelForEdit(SelectDeliveryModelForEditViewModel model)
        {
            var draft = GetStoredEditDraftApprenticeshipState();

            if (draft != null)
            {
                draft.DeliveryModel = (CommitmentsV2.Types.DeliveryModel?) model.DeliveryModel;
                StoreEditDraftApprenticeshipState(draft);

                return RedirectToAction(nameof(EditDraftApprenticeshipDisplay), draft);
            }

            if (model.DeliveryModel == null)
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail("DeliveryModel", "You must select the apprenticeship delivery model")});
            }

            return RedirectToAction(nameof(EditDraftApprenticeshipDisplay), model);
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/select-option")]
        public async Task<IActionResult> SelectOption(SelectOptionRequest request)
        {
            var model = await _modelMapper.Map<SelectOptionViewModel>(request);

            if (model == null)
            {
                return RedirectToAction("Details", "Cohort", new { request.AccountHashedId, request.CohortReference });
            }

            return View(model);
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/select-option")]
        public async Task<IActionResult> SelectOption(SelectOptionViewModel model)
        {
            var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipRequest>(model);

            await _commitmentsApiClient.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

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
            return RedirectToAction("Review", "Cohort", new { model.AccountHashedId });
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

        private void StoreDraftApprenticeshipState(AddDraftApprenticeshipViewModel model)
        {
            TempData.Put(nameof(AddDraftApprenticeshipViewModel), model);
        }

        private AddDraftApprenticeshipViewModel GetStoredDraftApprenticeshipState()
        {
            return TempData.Get<AddDraftApprenticeshipViewModel>(nameof(AddDraftApprenticeshipViewModel));
        }

        private void StoreEditDraftApprenticeshipState(EditDraftApprenticeshipViewModel model)
        {
            TempData.Put(nameof(EditDraftApprenticeshipViewModel), model);
        }

        private EditDraftApprenticeshipViewModel GetStoredEditDraftApprenticeshipState()
        {
            return TempData.Get<EditDraftApprenticeshipViewModel>(nameof(EditDraftApprenticeshipViewModel));
        }
    }
}