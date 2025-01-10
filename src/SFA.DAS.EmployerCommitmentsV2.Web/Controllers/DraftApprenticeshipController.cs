using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using SFA.DAS.Http;
using StructureMap.Query;
using System.Reflection;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.AccessDraftApprenticeship))]
[Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
public class DraftApprenticeshipController : Controller
{
    private readonly IModelMapper _modelMapper;
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;
    private readonly IApprovalsApiClient _outerApi;
    private readonly ICacheStorageService _cacheStorageService;

    private const string ApprenticeDeletedMessage = "Apprentice record deleted";

    public DraftApprenticeshipController(
        IModelMapper modelMapper,
        ICommitmentsApiClient commitmentsApiClient,
        IEncodingService encodingService,
        IApprovalsApiClient outerApi,
        ICacheStorageService cacheStorageService)
    {
        _modelMapper = modelMapper;
        _commitmentsApiClient = commitmentsApiClient;
        _encodingService = encodingService;
        _outerApi = outerApi;
        _cacheStorageService = cacheStorageService;
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
            var model = await GetStoredAddDraftApprenticeshipFromCache(request.CacheKey);
            if (model == null)
            {
                model = await _modelMapper.Map<AddDraftApprenticeshipViewModel>(request);
            }
            else
            {
                model.CourseCode = request.CourseCode;
                model.DeliveryModel = request.DeliveryModel;
                await AssignFundingDetailsToModel(model);
            }

            return View("AddDraftApprenticeship", model);
        }
        catch (CohortEmployerUpdateDeniedException)
        {
            return RedirectToAction("Details", "Cohort", new {request.AccountHashedId, request.CohortReference});
        }
    }

    [HttpPost]
    [Route("add-another")]
    public async Task<IActionResult> AddDraftApprenticeshipDetails(string changeCourse, string changeDeliveryModel,
        AddDraftApprenticeshipViewModel model)
    {
        if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
        {
            await StoreAddDraftApprenticeshipInCache(model, model.CacheKey);
            var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);
            return RedirectToAction(changeCourse == "Edit" ? nameof(SelectCourse) : nameof(SelectDeliveryModel),
                request.CloneBaseValues());
        }

        var addDraftApprenticeshipRequest = await _modelMapper.Map<AddDraftApprenticeshipApimRequest>(model);

        var response = await _outerApi.AddDraftApprenticeship(model.CohortId.Value, addDraftApprenticeshipRequest);

        await RemoveAddDraftApprenticeshipFromCache(model.CacheKey);

        var draftApprenticeshipHashedId =
            _encodingService.Encode(response.DraftApprenticeshipId, EncodingType.ApprenticeshipId);

        return RedirectToAction("SelectOption", "DraftApprenticeship",
            new {model.AccountHashedId, model.CohortReference, draftApprenticeshipHashedId});
    }

    [HttpGet]
    [Route("{DraftApprenticeshipHashedId}", Name = "Details")]
    [Route("{DraftApprenticeshipHashedId}/edit", Name = "Details-Edit")]
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
    [Route("{DraftApprenticeshipHashedId}/edit-display", Name = "EditDraftApprenticeshipDisplay")]
    public async Task<IActionResult> EditDraftApprenticeshipDisplay(EditDetailsRequest request)
    {
        var localModel = await GetStoredEditDraftApprenticeshipFromCache(request.CacheKey);

        if (localModel != null)
        {
            localModel.CourseCode = request.CourseCode;
            localModel.DeliveryModel = request.DeliveryModel;
            await AssignFundingDetailsToModel(localModel);
            return View("Edit", localModel);
        }

        var model = await _modelMapper.Map<IDraftApprenticeshipViewModel>(request);

        return View("Edit", model as EditDraftApprenticeshipViewModel);
    }

    [HttpPost]
    [Route("{DraftApprenticeshipHashedId}")]
    [Route("{DraftApprenticeshipHashedId}/edit")]
    [Route("{DraftApprenticeshipHashedId}/edit-display")]
    public async Task<IActionResult> EditDraftApprenticeship(string changeCourse, string changeDeliveryModel,
        EditDraftApprenticeshipViewModel model)
    {
        if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
        {
            await StoreEditDraftApprenticeshipInCache(model, model.CacheKey);
            var req = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);

            return RedirectToAction(
                changeCourse == "Edit" ? nameof(SelectCourseForEdit) : nameof(SelectDeliveryModelForEdit),
                req.CloneBaseValues());
        }

        var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipApimRequest>(model);

        await _outerApi.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

        await RemoveEditDraftApprenticeshipFromCache(model.CacheKey);

        return RedirectToAction("SelectOption", "DraftApprenticeship",
            new {model.AccountHashedId, model.CohortReference, model.DraftApprenticeshipHashedId});
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
                {new(nameof(model.CourseCode), "You must select a training course")});
        }

        var request = await _modelMapper.Map<AddDraftApprenticeshipRequest>(model);

        return RedirectToAction(nameof(SelectDeliveryModelForEdit), request.CloneBaseValues());
    }

    [HttpGet]
    [Route("{DraftApprenticeshipHashedId}/edit/select-delivery-model")]
    public async Task<IActionResult> SelectDeliveryModelForEdit(EditDetailsRequest request)
    {
        var model = await _modelMapper.Map<SelectDeliveryModelForEditViewModel>(request);

        if (model != null)
        {
            model.DeliveryModel = (DeliveryModel?) request.DeliveryModel;

            if (model.DeliveryModels.Count > 1 || model.HasUnavailableFlexiJobAgencyDeliveryModel)
            {
                return View(model);
            }

            request.DeliveryModel = (CommitmentsV2.Types.DeliveryModel) model.DeliveryModels.FirstOrDefault();
        }

        return RedirectToAction(nameof(EditDraftApprenticeshipDisplay),
            new
            {
                request.AccountHashedId, request.CohortReference, request.DraftApprenticeshipHashedId,
                request.DeliveryModel, request.CourseCode, request.CacheKey
            });
    }

    [HttpPost]
    [Route("{DraftApprenticeshipHashedId}/edit/select-delivery-model")]
    public async Task<IActionResult> SetDeliveryModelForEdit(SelectDeliveryModelForEditViewModel model)
    {
        var draft = await GetStoredEditDraftApprenticeshipFromCache(model.CacheKey);

        if (draft != null)
        {
            draft.HasChangedDeliveryModel =
                draft.DeliveryModel != (CommitmentsV2.Types.DeliveryModel?) model.DeliveryModel;
            draft.DeliveryModel = (CommitmentsV2.Types.DeliveryModel?) model.DeliveryModel;
            if (!string.IsNullOrWhiteSpace(model.CourseCode))
            {
                draft.CourseCode = model.CourseCode;
                await AssignFundingDetailsToModel(draft);
            }

            await StoreEditDraftApprenticeshipInCache(draft, model.CacheKey);
            return RedirectToAction(nameof(EditDraftApprenticeshipDisplay),
                new
                {
                    model.AccountHashedId, draft.CohortReference, draft.DraftApprenticeshipHashedId,
                    model.DeliveryModel, model.CourseCode, model.CacheKey
                });
        }

        if (model.DeliveryModel == null)
        {
            throw new CommitmentsApiModelException(new List<ErrorDetail>
                {new("DeliveryModel", "You must select the apprenticeship delivery model")});
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
            return RedirectToAction("Details", "Cohort", new {request.AccountHashedId, request.CohortReference});
        }
        return View(model);
    }

    [HttpPost]
    [Route("{DraftApprenticeshipHashedId}/select-option")]
    public async Task<IActionResult> SelectOption(SelectOptionViewModel model)
    {
        var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipApimRequest>(model);

        await _outerApi.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

        return RedirectToAction("Details", "Cohort", new {model.AccountHashedId, model.CohortReference});
    }

    [HttpGet]
    [Route("{DraftApprenticeshipHashedId}/Delete", Name = "DeleteDraftApprenticeship")]
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
            TempData.AddFlashMessage(ApprenticeDeletedMessage, TempDataDictionaryExtensions.FlashMessageLevel.Success);
            return await RedirectToCohortDetailsOrCohortsPage(model);
        }

        return RedirectToOriginForDelete(model.Origin, model.AccountHashedId, model.CohortReference, model.DraftApprenticeshipHashedId);
    }

    private async Task<IActionResult> RedirectToCohortDetailsOrCohortsPage(DeleteDraftApprenticeshipViewModel model)
    {
        if (await CohortExists(model.CohortId))
        {
            return RedirectToAction("Details", "Cohort", new {model.AccountHashedId, model.CohortReference});
        }

        return RedirectToAction("Review", "Cohort", new {model.AccountHashedId});
    }

    private async Task<bool> CohortExists(long? cohortId)
    {
        try
        {
            await _commitmentsApiClient.GetCohort(cohortId.Value);
            return true;
        }
        catch (RestHttpClientException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
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
        return origin == DeleteDraftApprenticeshipOrigin.CohortDetails
            ? RedirectToCohortDetails(accountHashedId, cohortReference)
            : RedirectToAction("Details", new {accountHashedId, cohortReference, draftApprenticeshipHashedId});
    }

    private IActionResult RedirectToCohortDetails(string accountHashedId, string cohortReference)
    {
        return RedirectToAction("Details", "Cohort", new {accountHashedId, cohortReference});
    }

    private async Task StoreAddDraftApprenticeshipInCache(AddDraftApprenticeshipViewModel model, Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.SaveToCache(key.Value, model, 1);
        }
    }

    private async Task RemoveAddDraftApprenticeshipFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.DeleteFromCache(key.Value);
        }
    }

    private async Task<AddDraftApprenticeshipViewModel> GetStoredAddDraftApprenticeshipFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            return await _cacheStorageService.RetrieveFromCache<AddDraftApprenticeshipViewModel>(key.Value);
        }

        return null;
    }

    private async Task StoreEditDraftApprenticeshipInCache(EditDraftApprenticeshipViewModel model, Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.SaveToCache(key.Value, model, 1);
        }
    }

    private async Task RemoveEditDraftApprenticeshipFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.DeleteFromCache(key.Value);
        }
    }

    private async Task<EditDraftApprenticeshipViewModel> GetStoredEditDraftApprenticeshipFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            return await _cacheStorageService.RetrieveFromCache<EditDraftApprenticeshipViewModel>(key.Value);
        }

        return null;
    }

    private async Task AssignFundingDetailsToModel(DraftApprenticeshipViewModel model)
    {
        if (!string.IsNullOrEmpty(model?.CourseCode))
        {
            var fundingBandData = await _outerApi.GetFundingBandDataByCourseCodeAndStartDate(model.CourseCode, model.StartDate.Date);
            model.FundingBandMax = fundingBandData?.ProposedMaxFunding;
            model.StandardPageUrl = fundingBandData?.StandardPageUrl;
        }
    }
}