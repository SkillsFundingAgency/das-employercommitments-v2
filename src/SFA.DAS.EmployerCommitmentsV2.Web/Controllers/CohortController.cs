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
            AddApprenticeshipCacheKey = Guid.NewGuid(),
            AccountHashedId = request.AccountHashedId,
            AccountId = request.AccountId,
            ReservationId = request.ReservationId,
            AccountLegalEntityHashedId = request.AccountLegalEntityHashedId,
            StartMonthYear = request.StartMonthYear,
            CourseCode = request.CourseCode
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        var viewModel = await _modelMapper.Map<IndexViewModel>(request);
        viewModel.AddApprenticeshipCacheKey = cacheModel.AddApprenticeshipCacheKey;

        return View(viewModel);
    }

    [Route("add/select-provider")]
    public async Task<IActionResult> SelectProvider(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);

        _encodingService.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out var id);
        cacheModel.AccountLegalEntityId = id;

        var viewModel = await _modelMapper.Map<SelectProviderViewModel>(cacheModel);

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return View(viewModel);
    }

    [Route("add/select-provider")]
    [HttpPost]
    public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
    {
        try
        {
            var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.AddApprenticeshipCacheKey);

            await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));

            cacheModel.ProviderId = long.Parse(request.ProviderId);
            await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

            return RedirectToAction(RouteNames.CohortConfirmProvider, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
        }
        catch (RestHttpClientException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(nameof(request.ProviderId), "Select a training provider");
                return RedirectToAction(RouteNames.CohortSelectProvider, new { request.AccountHashedId, request.AddApprenticeshipCacheKey });
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
    public async Task<IActionResult> ConfirmProvider(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);

        var model = await _modelMapper.Map<ConfirmProviderViewModel>(cacheModel);
        return View(model);
    }

    [Route("add/confirm-provider")]
    [HttpPost]
    public IActionResult ConfirmProvider(ConfirmProviderViewModel model)
    {
        if (model.UseThisProvider.Value)
        {
            return RedirectToAction(RouteNames.CohortAssign, new { model.AccountHashedId, model.AddApprenticeshipCacheKey });
        }

        return RedirectToAction(RouteNames.CohortSelectProvider, new { model.AccountHashedId, model.AddApprenticeshipCacheKey });
    }

    [Route("add/assign")]
    public async Task<IActionResult> Assign(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);
        var viewModel = await _modelMapper.Map<AssignViewModel>(cacheModel);
        return View(viewModel);
    }

    [Route("add/assign")]
    [HttpPost]
    public async Task<IActionResult> Assign(AssignViewModel model)
    {
        if (!model.ReservationId.HasValue && model.WhoIsAddingApprentices == WhoIsAddingApprentices.Employer)
        {
            var url = _linkGenerator.ReservationsLink(
                $"accounts/{model.AccountHashedId}/reservations/{model.AccountLegalEntityHashedId}/select?" +
                $"providerId={model.ProviderId}" +
                $"&transferSenderId={model.TransferSenderId}" +
                $"&encodedPledgeApplicationId={model.EncodedPledgeApplicationId}" +
                $"&approvalsCacheKey={model.AddApprenticeshipCacheKey}");
            return Redirect(url);
        }

        if (model.WhoIsAddingApprentices == WhoIsAddingApprentices.Employer)
        {
            model.Message = string.Empty;
        }

        switch (model.WhoIsAddingApprentices)
        {
            case WhoIsAddingApprentices.Employer:
                var routeValues = new
                {
                    model.AccountHashedId,
                    model.AccountLegalEntityHashedId,
                    model.ReservationId,
                    model.StartMonthYear,
                    model.CourseCode,
                    model.ProviderId,
                    model.TransferSenderId,
                    model.EncodedPledgeApplicationId,
                    model.AddApprenticeshipCacheKey
                };
                return RedirectToAction(RouteNames.CohortApprentice, "Cohort", routeValues);
            case WhoIsAddingApprentices.Provider:
                var request = await _modelMapper.Map<CreateCohortWithOtherPartyRequest>(model);
                var response = await _commitmentsApiClient.CreateCohort(request);
                return RedirectToAction("Finished", new { model.AccountHashedId, response.CohortReference });
            default:
                return RedirectToAction("Error", "Error");
        }
    }

    [HttpGet]
    [Route("add/apprentice")]
    public async Task<IActionResult> Apprentice(ApprenticeRequest request)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.AddApprenticeshipCacheKey.Value);
        cacheModel.ReservationId = request.ReservationId;
        cacheModel.CourseCode = request.CourseCode;
        cacheModel.StartMonthYear = request.StartMonthYear;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(RouteNames.CohortSelectCourse, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpGet]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);

        var selectCourseViewModel = await _modelMapper.Map<SelectCourseViewModel>(cacheModel);
        return View(RouteNames.CohortSelectCourse, selectCourseViewModel);
    }

    [HttpPost]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse(SelectCourseViewModel model)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.AddApprenticeshipCacheKey);
        cacheModel.CourseCode = model.CourseCode;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(RouteNames.CohortSelectDeliveryModel, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpGet]
    [ActionName(RouteNames.CohortSelectDeliveryModel)]
    [Route("add/select-delivery-model")]
    public async Task<IActionResult> SelectDeliveryModel(ApprenticeRequest request)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(request.AddApprenticeshipCacheKey.Value);

        cacheModel.ReservationId = request.ReservationId.HasValue ? request.ReservationId : cacheModel.ReservationId;
        cacheModel.CourseCode = !string.IsNullOrEmpty(request.CourseCode) ? request.CourseCode : cacheModel.CourseCode;
        cacheModel.StartMonthYear = !string.IsNullOrEmpty(request.StartMonthYear) ? request.StartMonthYear : cacheModel.StartMonthYear;

        var model = await _modelMapper.Map<SelectDeliveryModelViewModel>(cacheModel);

        if (model.DeliveryModels.Length > 1)
        {
            await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

            return View(RouteNames.CohortSelectDeliveryModel, model);
        }

        cacheModel.DeliveryModel = model.DeliveryModels.FirstOrDefault();

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(nameof(AddDraftApprenticeship), new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpPost]
    [Route("add/select-delivery-model")]
    public async Task<IActionResult> SetDeliveryModel(SelectDeliveryModelViewModel model)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.AddApprenticeshipCacheKey.Value);

        if (model.DeliveryModel == null)
        {
            throw new CommitmentsApiModelException(new List<ErrorDetail>
                {new ErrorDetail("DeliveryModel", "You must select the apprenticeship delivery model")});
        }

        cacheModel.DeliveryModel = model.DeliveryModel;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(nameof(AddDraftApprenticeship), new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpGet]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeship(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);

        var model = await _modelMapper.Map<ApprenticeViewModel>(cacheModel);

        cacheModel.LegalEntityName = model.LegalEntityName;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return View(RouteNames.CohortApprentice, model);
    }

    [HttpPost]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeshipOrRoute(string changeCourse, string changeDeliveryModel, ApprenticeViewModel model)
    {
        if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
        {
            var cacheModel = await GetAddApprenticeshipCacheModelFromCache(model.AddApprenticeshipCacheKey);

            UpdateAddApprenticeshipCacheModelFromApprenticeViewModel(cacheModel, model);

            await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

            return RedirectToAction(changeCourse == "Edit" ? RouteNames.CohortSelectCourse : RouteNames.CohortSelectDeliveryModel,
                new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
        }

        return await SaveDraftApprenticeship(model);
    }

    public async Task<IActionResult> SaveDraftApprenticeship(ApprenticeViewModel model)
    {
        var request = await _modelMapper.Map<CreateCohortApimRequest>(model);
        var newCohort = await _approvalsApiClient.CreateCohort(request);

        await RemoveAddApprenticeshipCacheModelFromCache(model.AddApprenticeshipCacheKey);

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
            AddApprenticeshipCacheKey = Guid.NewGuid(),
            AccountHashedId = request.AccountHashedId,
            AccountId = request.AccountId
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        var viewModel = await _modelMapper.Map<InformViewModel>(request);

        viewModel.AddApprenticeshipCacheKey = cacheModel.AddApprenticeshipCacheKey;

        return View(viewModel);
    }

    [HttpGet]
    [Route("transferConnection/create")]
    public async Task<IActionResult> SelectTransferConnection(Guid addApprenticeshipCacheKey)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey);

        var viewModel = await _modelMapper.Map<SelectTransferConnectionViewModel>(cacheModel);

        if (viewModel.TransferConnections.Count > 0)
        {
            return View(viewModel);
        }

        cacheModel.TransferSenderId = string.Empty;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(RouteNames.CohortSelectLegalEntity, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpPost]
    [Route("transferConnection/create")]
    public async Task<IActionResult> SetTransferConnection(SelectTransferConnectionViewModel selectedTransferConnection)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(selectedTransferConnection.AddApprenticeshipCacheKey);

        var transferConnectionCode = selectedTransferConnection.TransferConnectionCode.Equals("None", StringComparison.InvariantCultureIgnoreCase)
            ? null : selectedTransferConnection.TransferConnectionCode;

        cacheModel.TransferSenderId = transferConnectionCode;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        return RedirectToAction(RouteNames.CohortSelectLegalEntity, new { selectedTransferConnection.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
    }

    [HttpGet]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<IActionResult> SelectLegalEntity([FromRoute] string accountHashedId, Guid? addApprenticeshipCacheKey, string encodedPledgeApplicationId = null, string transferConnectionCode = null)
    {
        var cacheModel = addApprenticeshipCacheKey.HasValue
        ? await GetAddApprenticeshipCacheModelFromCache(addApprenticeshipCacheKey.Value)
        : await CreateAndStoreNewCacheModel(accountHashedId, encodedPledgeApplicationId, transferConnectionCode);

        var response = await _modelMapper.Map<SelectLegalEntityViewModel>(cacheModel);

        if (response.LegalEntities == null || !response.LegalEntities.Any())
            throw new Exception($"No legal entities associated with account {cacheModel.AccountHashedId}");

        if (response.LegalEntities.Count() > 1)
            return View(response);

        var autoSelectLegalEntity = response.LegalEntities.First();

        var hasSignedMinimumRequiredAgreementVersion = autoSelectLegalEntity.HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(cacheModel.TransferSenderId));

        cacheModel.AccountLegalEntityHashedId = autoSelectLegalEntity.AccountLegalEntityPublicHashedId;

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        if (hasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
        }

        var model = new LegalEntitySignedAgreementViewModel
        {
            AccountHashedId = cacheModel.AccountHashedId,
            AccountLegalEntityId = autoSelectLegalEntity.Id,
            CohortRef = cacheModel.CohortRef,
            LegalEntityName = autoSelectLegalEntity.Name,
            TransferConnectionCode = cacheModel.TransferSenderId,
            AccountLegalEntityHashedId = cacheModel.AccountLegalEntityHashedId
        };

        return RedirectToAction(RouteNames.CohortAgreementNotSigned, model.CloneBaseValues());

    }

    [HttpPost]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<ActionResult> SetLegalEntity(SelectLegalEntityViewModel selectedLegalEntity)
    {
        var cacheModel = await GetAddApprenticeshipCacheModelFromCache(selectedLegalEntity.AddApprenticeshipCacheKey);

        var response = await _modelMapper.Map<LegalEntitySignedAgreementViewModel>(selectedLegalEntity);

        cacheModel.AccountLegalEntityHashedId = response.AccountLegalEntityHashedId;
        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);

        if (response.HasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction(RouteNames.CohortSelectProvider, new { cacheModel.AccountHashedId, cacheModel.AddApprenticeshipCacheKey });
        }

        var model = new LegalEntitySignedAgreementViewModel
        {
            AccountHashedId = selectedLegalEntity.AccountHashedId,
            AccountLegalEntityId = selectedLegalEntity.LegalEntityId,
            CohortRef = selectedLegalEntity.CohortRef,
            LegalEntityName = response.LegalEntityName,
            AccountLegalEntityHashedId = response.AccountLegalEntityHashedId,
            EncodedPledgeApplicationId = selectedLegalEntity.EncodedPledgeApplicationId
        };

        return RedirectToAction(RouteNames.CohortAgreementNotSigned, model.CloneBaseValues());
    }

    [HttpGet]
    [Route(RouteNames.CohortAgreementNotSigned)]
    public async Task<ActionResult> AgreementNotSigned(LegalEntitySignedAgreementViewModel viewModel)
    {
        var response = await _modelMapper.Map<AgreementNotSignedViewModel>(viewModel);

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
            throw new MissingAddApprenticeshipCacheKeyException();
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

    //This is needed when joining from LTM
    private async Task<AddApprenticeshipCacheModel> CreateAndStoreNewCacheModel(string accountHashedId, string encodedPledgeApplicationId, string transferConnectionCode)
    {
        var cacheModel = new AddApprenticeshipCacheModel
        {
            AddApprenticeshipCacheKey = Guid.NewGuid(),
            AccountHashedId = accountHashedId,
            AccountId = _encodingService.Decode(accountHashedId, EncodingType.ApprenticeshipId),
            TransferSenderId = transferConnectionCode,
            EncodedPledgeApplicationId = encodedPledgeApplicationId
        };

        await StoreAddApprenticeshipCacheModelInCache(cacheModel, cacheModel.AddApprenticeshipCacheKey);
        return cacheModel;
    }
}