using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers;

[Authorize(Policy = nameof(PolicyNames.HasEmployerTransactorOwnerAccount))]
[Route("{accountHashedId}/unapproved")]
public class CohortController : Controller
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly ILogger<CohortController> _logger;
    private readonly ILinkGenerator _linkGenerator;
    private readonly IModelMapper _modelMapper;
    private readonly IEncodingService _encodingService;
    private readonly IApprovalsApiClient _approvalsApiClient;
    private readonly ICacheStorageService _cacheStorageService;

    public CohortController(
        ICommitmentsApiClient commitmentsApiClient,
        ILogger<CohortController> logger,
        ILinkGenerator linkGenerator,
        IModelMapper modelMapper,
        IEncodingService encodingService,
        IApprovalsApiClient approvalsApiClient,
        ICacheStorageService cacheStorageService)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _logger = logger;
        _linkGenerator = linkGenerator;
        _modelMapper = modelMapper;
        _encodingService = encodingService;
        _approvalsApiClient = approvalsApiClient;
        _cacheStorageService = cacheStorageService;
    }

    [Route("{cohortReference}")]
    [Route("{cohortReference}/details")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    public async Task<IActionResult> Details(DetailsRequest request)
    {
        var viewModel = await _modelMapper.Map<DetailsViewModel>(request);

        return View(viewModel);
    }

    [Route("{cohortReference}")]
    [Route("{cohortReference}/details")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    [HttpPost]
    public async Task<IActionResult> Details(DetailsViewModel viewModel)
    {
        switch (viewModel.Selection)
        {
            case CohortDetailsOptions.Send:
            case CohortDetailsOptions.Approve:
                {
                    var request = await _modelMapper.Map<AcknowledgementRequest>(viewModel);
                    var acknowledgementAction = viewModel.Selection == CohortDetailsOptions.Approve ? "Approved" : "Sent";
                    return RedirectToAction(acknowledgementAction, request);
                }
            case CohortDetailsOptions.ViewEmployerAgreement:
                {
                    var request = await _modelMapper.Map<ViewEmployerAgreementRequest>(viewModel);
                    return ViewEmployeeAgreementRedirect(request);
                }
            case CohortDetailsOptions.Homepage:
                {
                    return Redirect(_linkGenerator.AccountsLink($"accounts/{viewModel.AccountHashedId}/teams"));
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(viewModel.Selection));
        }
    }

    [HttpGet]
    [Route("viewAgreement", Name = "ViewAgreement")]
    public async Task<IActionResult> ViewAgreement(string accountHashedId, int? cohortId)
    {
        var request = !cohortId.HasValue
            ? new ViewEmployerAgreementRequest { AccountHashedId = accountHashedId }
            : await _modelMapper.Map<ViewEmployerAgreementRequest>(new DetailsViewModel
            {
                AccountHashedId = accountHashedId,
                CohortId = cohortId.Value
            });

        return ViewEmployeeAgreementRedirect(request);
    }

    private IActionResult ViewEmployeeAgreementRedirect(ViewEmployerAgreementRequest request)
    {
        if (request.AgreementHashedId == null)
        {
            return Redirect(_linkGenerator.AccountsLink($"accounts/{request.AccountHashedId}/agreements/"));
        }
        return Redirect(_linkGenerator.AccountsLink(
            $"accounts/{request.AccountHashedId}/agreements/{request.AgreementHashedId}/about-your-agreement"));
    }

    [Route("{cohortReference}/delete")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    public async Task<IActionResult> ConfirmDelete(DetailsRequest request)
    {
        var viewModel = await _modelMapper.Map<ConfirmDeleteViewModel>(request);
        return View(viewModel);
    }

    [Route("{cohortReference}/delete")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    [HttpPost]
    public async Task<IActionResult> Delete([FromServices] IAuthenticationService authenticationService, ConfirmDeleteViewModel viewModel)
    {
        if (viewModel.ConfirmDeletion == true)
        {
            await _commitmentsApiClient.DeleteCohort(viewModel.CohortId, authenticationService.UserInfo, CancellationToken.None);
            return RedirectToAction(RouteNames.CohortReview, new { viewModel.CohortReference, viewModel.AccountHashedId });
        }
        return RedirectToAction(RouteNames.CohortDetails, new { viewModel.CohortReference, viewModel.AccountHashedId });
    }

    [HttpGet]
    [Route("{cohortReference}/sent")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    public async Task<IActionResult> Sent(SentRequest request)
    {
        var viewModel = await _modelMapper.Map<SentViewModel>(request);
        return View(viewModel);
    }

    [HttpGet]
    [Route("{cohortReference}/approved")]
    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    public async Task<IActionResult> Approved(ApprovedRequest request)
    {
        var viewModel = await _modelMapper.Map<ApprovedViewModel>(request);
        return View(viewModel);
    }

    [Route("add")]
    public async Task<IActionResult> Index(IndexRequest request)
    {
        var cacheModel = new AddApprenticeshipCacheModel
        {
            ApprenticeshipSessionKey = Guid.NewGuid(),
            AccountHashedId = request.AccountHashedId,
            AccountId = request.AccountId,
            ReservationId = request.ReservationId,
            AccountLegalEntityHashedId = request.AccountLegalEntityHashedId,
            StartMonthYear = request.StartMonthYear,
            CourseCode = request.CourseCode
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        var viewModel = await _modelMapper.Map<IndexViewModel>(request);
        viewModel.ApprenticeshipSessionKey = cacheModel.ApprenticeshipSessionKey;

        return View(viewModel);
    }

    [Route("add/select-provider")]
    public async Task<IActionResult> SelectProvider([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        _encodingService.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out var id);
        cacheModel.AccountLegalEntityId = id;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        var viewModel = await _modelMapper.Map<SelectProviderViewModel>(cacheModel);

        return View(viewModel);
    }

    [Route("add/select-provider")]
    [HttpPost]
    public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
    {
        try
        {
            var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.ApprenticeshipSessionKey);

            await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));
            cacheModel.ProviderId = long.Parse(request.ProviderId);
            cacheModel.LegalEntityName = request.LegalEntityName;
            await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

            return RedirectToAction(RouteNames.CohortConfirmProvider, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }
        catch (RestHttpClientException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(nameof(request.ProviderId), "Select a training provider");
                return RedirectToAction(RouteNames.CohortSelectProvider, new { request.AccountHashedId, request.ApprenticeshipSessionKey });
            }

            _logger.LogError(ex, "Failed '{ControllerName)}.{ActionName}'", nameof(CohortController), nameof(SelectProvider));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed '{ControllerName)}.{ActionName}'", nameof(CohortController), nameof(SelectProvider));
        }

        return RedirectToAction("Error", "Error");
    }

    [Route("add/confirm-provider")]
    [HttpGet]
    public async Task<IActionResult> ConfirmProvider([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        var model = await _modelMapper.Map<ConfirmProviderViewModel>(cacheModel);
        return View(model);
    }

    [Route("add/confirm-provider")]
    [HttpPost]
    public IActionResult ConfirmProvider(ConfirmProviderViewModel model)
    {
        if (model.UseThisProvider.Value)
        {
            return RedirectToAction(RouteNames.CohortAssign, new { model.AccountHashedId, model.ApprenticeshipSessionKey });
        }

        return RedirectToAction(RouteNames.CohortSelectProvider, new { model.AccountHashedId, model.ApprenticeshipSessionKey });
    }

    [Route("add/assign")]
    public async Task<IActionResult> Assign([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);
        var viewModel = await _modelMapper.Map<AssignViewModel>(cacheModel);
        return View(viewModel);
    }

    [Route("add/assign")]
    [HttpPost]
    public async Task<IActionResult> Assign(AssignViewModel model)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.ApprenticeshipSessionKey);
        cacheModel.Message = model.Message;

        bool NeedsToGetAReservation()
        {
            if (model.WhoIsAddingApprentices != WhoIsAddingApprentices.Employer)
                return false;
            if (cacheModel.ReservationId.HasValue)
                return false;
            if (cacheModel.FundingType == FundingType.AdditionalReservations)
                return false;
            return true;
        }     

        if (NeedsToGetAReservation())
        {
            var url = _linkGenerator.ReservationsLink(
                $"accounts/{cacheModel.AccountHashedId}/reservations/{cacheModel.AccountLegalEntityHashedId}/select?" +
                $"providerId={cacheModel.ProviderId}" +
                $"&transferSenderId={cacheModel.TransferSenderId}" +
                $"&encodedPledgeApplicationId={cacheModel.EncodedPledgeApplicationId}" +
                $"&apprenticeshipSessionKey={cacheModel.ApprenticeshipSessionKey}");
            return Redirect(url);
        }

        if (model.WhoIsAddingApprentices == WhoIsAddingApprentices.Employer)
        {
            model.Message = string.Empty;
        }

        switch (model.WhoIsAddingApprentices)
        {
            case WhoIsAddingApprentices.Employer:               
                return RedirectToAction(RouteNames.CohortApprentice, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
            case WhoIsAddingApprentices.Provider:
                var request = await _modelMapper.Map<CreateCohortWithOtherPartyRequest>(cacheModel);
                var response = await _commitmentsApiClient.CreateCohort(request);
                return RedirectToAction("Finished", new { cacheModel.AccountHashedId, response.CohortReference });
            default:
                return RedirectToAction("Error", "Error");
        }
    }

    [HttpGet]
    [Route("add/apprentice")]
    public async Task<IActionResult> Apprentice(ApprenticeRequest request)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.ApprenticeshipSessionKey);
        UpdateAddApprenticeshipCacheModelFromApprenticeRequest(request, cacheModel);

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        return RedirectToAction(RouteNames.CohortSelectCourse, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        var selectCourseViewModel = await _modelMapper.Map<SelectCourseViewModel>(cacheModel);
        return View(RouteNames.CohortSelectCourse, selectCourseViewModel);
    }

    [HttpPost]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse(SelectCourseViewModel model)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.ApprenticeshipSessionKey);
        cacheModel.CourseCode = model.CourseCode;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        return RedirectToAction(RouteNames.CohortSelectDeliveryModel, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [ActionName(RouteNames.CohortSelectDeliveryModel)]
    [Route("add/select-delivery-model")]
    public async Task<IActionResult> SelectDeliveryModel(ApprenticeRequest request)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.ApprenticeshipSessionKey);
        UpdateAddApprenticeshipCacheModelFromApprenticeRequest(request, cacheModel);

        var model = await _modelMapper.Map<SelectDeliveryModelViewModel>(cacheModel);

        cacheModel.DeliveryModel = model.DeliveryModels.Length > 1 ? null : model.DeliveryModels.FirstOrDefault();
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        if (model.DeliveryModels.Length > 1)
        {
            return View(RouteNames.CohortSelectDeliveryModel, model);
        }

        return RedirectToAction(nameof(AddDraftApprenticeship), new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpPost]
    [Route("add/select-delivery-model")]
    public async Task<IActionResult> SetDeliveryModel(SelectDeliveryModelViewModel model)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.ApprenticeshipSessionKey);

        if (model.DeliveryModel == null)
        {
            throw new CommitmentsApiModelException(new List<ErrorDetail>
                {new ErrorDetail("DeliveryModel", "You must select the apprenticeship delivery model")});
        }

        cacheModel.DeliveryModel = model.DeliveryModel;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        return RedirectToAction(nameof(AddDraftApprenticeship), new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeship([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        var model = await _modelMapper.Map<ApprenticeViewModel>(cacheModel);

        return View(RouteNames.CohortApprentice, model);
    }

    [HttpPost]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeshipOrRoute(string changeCourse, string changeDeliveryModel, ApprenticeViewModel model)
    {
        if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
        {
            var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.ApprenticeshipSessionKey);

            UpdateAddApprenticeshipCacheModelFromApprenticeViewModel(cacheModel, model);

            await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

            return RedirectToAction(changeCourse == "Edit" ? RouteNames.CohortSelectCourse : RouteNames.CohortSelectDeliveryModel,
                new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }

        return await SaveDraftApprenticeship(model);
    }

    public async Task<IActionResult> SaveDraftApprenticeship(ApprenticeViewModel model)
    {
        var request = await _modelMapper.Map<CreateCohortApimRequest>(model);
        var newCohort = await _approvalsApiClient.CreateCohort(request);

        await RemoveAddApprenticeshipCacheModelFromCache(model.ApprenticeshipSessionKey);

        var draftApprenticeshipsResponse = await _commitmentsApiClient.GetDraftApprenticeships(newCohort.CohortId);

        var draftApprenticeship = draftApprenticeshipsResponse.DraftApprenticeships.SingleOrDefault();

        if (draftApprenticeship?.CourseCode != null)
        {
            var draftApprenticeshipHashedId = _encodingService.Encode(draftApprenticeship.Id, EncodingType.ApprenticeshipId);

            return RedirectToAction("SelectOption", "DraftApprenticeship", new { model.AccountHashedId, newCohort.CohortReference, draftApprenticeshipHashedId });
        }

        return RedirectToAction(RouteNames.CohortDetails, new { model.AccountHashedId, newCohort.CohortReference });
    }

    [Authorize(Policy = nameof(PolicyNames.AccessCohort))]
    [HttpGet]
    [Route("add/finished")]
    public async Task<IActionResult> Finished(FinishedRequest request)
    {
        var response = await _commitmentsApiClient.GetCohort(request.CohortId);

        return View(new FinishedViewModel
        {
            AccountHashedId = request.AccountHashedId,
            CohortReference = request.CohortReference,
            LegalEntityName = response.LegalEntityName,
            ProviderName = response.ProviderName,
            Message = response.LatestMessageCreatedByEmployer
        });
    }

    [HttpGet]
    [Route(RouteNames.CohortReview, Name = RouteNames.CohortReview)]
    [Route("")]
    public async Task<IActionResult> Review(CohortsByAccountRequest request)
    {
        var reviewViewModel = await _modelMapper.Map<ReviewViewModel>(request);
        return View(reviewViewModel);
    }

    [HttpGet]
    [Route("draft")]
    public async Task<IActionResult> Draft(CohortsByAccountRequest request)
    {
        var viewModel = await _modelMapper.Map<DraftViewModel>(request);
        return View(viewModel);
    }

    [HttpGet]
    [Route("with-training-provider")]
    public async Task<IActionResult> WithTrainingProvider(CohortsByAccountRequest request)
    {
        var viewModel = await _modelMapper.Map<WithTrainingProviderViewModel>(request);
        return View(viewModel);
    }

    [HttpGet]
    [Route("with-transfer-sender")]
    public async Task<IActionResult> WithTransferSender(CohortsByAccountRequest request)
    {
        var viewModel = await _modelMapper.Map<WithTransferSenderViewModel>(request);
        return View(viewModel);
    }

    [HttpGet]
    [Route("Inform", Name = "Inform")]
    public async Task<ActionResult> Inform(InformRequest request)
    {
        var cacheModel = new AddApprenticeshipCacheModel
        {
            ApprenticeshipSessionKey = Guid.NewGuid(),
            AccountHashedId = request.AccountHashedId,
            AccountId = request.AccountId
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        var viewModel = await _modelMapper.Map<InformViewModel>(request);

        viewModel.ApprenticeshipSessionKey = cacheModel.ApprenticeshipSessionKey;

        return View(viewModel);
    }

    [HttpGet]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<IActionResult> SelectLegalEntity(
        [FromRoute] string accountHashedId,
        [FromQuery] Guid? apprenticeshipSessionKey,
        [FromQuery] string encodedPledgeApplicationId = null,
        [FromQuery] string transferConnectionCode = null)
    {
        var cacheModel = apprenticeshipSessionKey.HasValue
        ? await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey)
        : await CreateAndStoreNewCacheModelFromLevyTransfer(accountHashedId, encodedPledgeApplicationId, transferConnectionCode);

        var response = await _modelMapper.Map<SelectLegalEntityViewModel>(cacheModel);

        if (response.LegalEntities == null || !response.LegalEntities.Any())
            throw new Exception($"No legal entities associated with account {cacheModel.AccountHashedId}");

        if (response.LegalEntities.Count() > 1)
            return View(response);

        var autoSelectLegalEntity = response.LegalEntities.First();

        var hasSignedMinimumRequiredAgreementVersion = autoSelectLegalEntity.HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(cacheModel.TransferSenderId));

        cacheModel.AccountLegalEntityHashedId = autoSelectLegalEntity.AccountLegalEntityPublicHashedId;
        cacheModel.LegalEntityName = autoSelectLegalEntity.Name;
        cacheModel.AccountLegalEntityId = autoSelectLegalEntity.Id;

        cacheModel.HasSignedMinimumRequiredAgreementVersion = hasSignedMinimumRequiredAgreementVersion;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        if (hasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction(RouteNames.CohortSelectFunding, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }

        return RedirectToAction(RouteNames.CohortAgreementNotSigned, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpPost]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<ActionResult> SelectLegalEntity(SelectLegalEntityViewModel selectedLegalEntity)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(selectedLegalEntity.ApprenticeshipSessionKey);

        var response = await _modelMapper.Map<LegalEntitySignedAgreementViewModel>(selectedLegalEntity);

        cacheModel.AccountLegalEntityHashedId = response.AccountLegalEntityHashedId;
        cacheModel.AccountLegalEntityId = response.AccountLegalEntityId;
        cacheModel.LegalEntityName = response.LegalEntityName;
        cacheModel.HasSignedMinimumRequiredAgreementVersion = response.HasSignedMinimumRequiredAgreementVersion;
        cacheModel.CohortRef = response.CohortRef;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        if (response.HasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction(RouteNames.CohortSelectFunding, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }

        return RedirectToAction(RouteNames.CohortAgreementNotSigned, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [Route("add/select-funding")]
    public async Task<IActionResult> SelectFunding([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        if (cacheModel.EncodedPledgeApplicationId != null || cacheModel.TransferSenderId != null)
        {
            return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }

        var viewModel = await _modelMapper.Map<SelectFundingViewModel>(cacheModel);

        if (viewModel.HasDirectTransfersAvailable == false &&
             viewModel.HasAdditionalReservationFundsAvailable == false &&
             viewModel.HasUnallocatedReservationsAvailable == false)
        {
            return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }
        return View(viewModel);
    }

    [HttpPost]
    [Route("add/select-funding")]
    public async Task<ActionResult> SelectFundingType(SelectFundingViewModel selectedFunding)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(selectedFunding.ApprenticeshipSessionKey);
        cacheModel.FundingType = selectedFunding.FundingType;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);       

        if (selectedFunding.FundingType == FundingType.DirectTransfers)
        {
            return RedirectToAction(RouteNames.CohortSelectDirectTransferConnection, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
        }
        return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [Route("add/select-funding/select-direct-connection")]
    public async Task<IActionResult> SelectDirectTransferConnection([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);
        var viewModel = await _modelMapper.Map<SelectTransferConnectionViewModel>(cacheModel);

        return View(viewModel);
    }

    [HttpPost]
    [Route("add/select-funding/select-direct-connection")]
    public async Task<IActionResult> SelectDirectTransferConnection(SelectTransferConnectionViewModel selectedTransferConnection)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(selectedTransferConnection.ApprenticeshipSessionKey);
        var transferConnectionCode = selectedTransferConnection.TransferConnectionCode;       
        cacheModel.TransferSenderId = transferConnectionCode;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);

        return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.ApprenticeshipSessionKey });
    }

    [HttpGet]
    [Route(RouteNames.CohortAgreementNotSigned)]
    public async Task<ActionResult> AgreementNotSigned([FromQuery] Guid apprenticeshipSessionKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(apprenticeshipSessionKey);

        var response = await _modelMapper.Map<AgreementNotSignedViewModel>(cacheModel);

        return View(response);
    }

    private async Task StoreAddApprenticeshipCacheModelInCache(AddApprenticeshipCacheModel model, Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.SaveToCache(key.Value, model, 1);
        }
    }

    private async Task<AddApprenticeshipCacheModel> GetAddApprenticeshipCacheModelFromCache(Guid? key)
    {
        if (!key.IsNotNullOrEmpty())
        {
            throw new MissingApprenticeshipSessionKeyException();
        }

        return await _cacheStorageService.RetrieveFromCache<AddApprenticeshipCacheModel>(key.Value);
    }

    private async Task RemoveAddApprenticeshipCacheModelFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.DeleteFromCache(key.Value);
        }
    }

    private static void UpdateAddApprenticeshipCacheModelFromApprenticeViewModel(AddApprenticeshipCacheModel cacheModel, ApprenticeViewModel model)
    {
        cacheModel.FirstName = model.FirstName;
        cacheModel.LastName = model.LastName;
        cacheModel.Email = model.Email;
        cacheModel.BirthDay = model.BirthDay;
        cacheModel.BirthMonth = model.BirthMonth;
        cacheModel.BirthYear = model.BirthYear;
        cacheModel.StartMonth = model.StartMonth;
        cacheModel.StartYear = model.StartYear;
        cacheModel.EndMonth = model.EndMonth;
        cacheModel.EndYear = model.EndYear;
        cacheModel.EmploymentEndMonth = model.EmploymentEndMonth;
        cacheModel.EmploymentEndYear = model.EmploymentEndYear;
        cacheModel.Cost = model.Cost;
        cacheModel.EmploymentPrice = model.EmploymentPrice;
        cacheModel.Reference = model.Reference;
    }

    private async Task<AddApprenticeshipCacheModel> CreateAndStoreNewCacheModelFromLevyTransfer(string accountHashedId, string encodedPledgeApplicationId, string transferConnectionCode)
    {
        var cacheModel = new AddApprenticeshipCacheModel
        {
            ApprenticeshipSessionKey = Guid.NewGuid(),
            AccountHashedId = accountHashedId,
            AccountId = _encodingService.Decode(accountHashedId, EncodingType.ApprenticeshipId),
            TransferSenderId = transferConnectionCode,
            EncodedPledgeApplicationId = encodedPledgeApplicationId
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.ApprenticeshipSessionKey);
        return cacheModel;
    }

    private static void UpdateAddApprenticeshipCacheModelFromApprenticeRequest(ApprenticeRequest request, AddApprenticeshipCacheModel cacheModel)
    {
        cacheModel.ReservationId = request.ReservationId ?? cacheModel.ReservationId;
        cacheModel.CourseCode = !string.IsNullOrEmpty(request.CourseCode) ? request.CourseCode : cacheModel.CourseCode;
        cacheModel.StartMonthYear = !string.IsNullOrEmpty(request.StartMonthYear) ? request.StartMonthYear : cacheModel.StartMonthYear;
    }

}