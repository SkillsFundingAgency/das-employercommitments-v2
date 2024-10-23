using Microsoft.AspNetCore.Authorization;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
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
    public async Task<IActionResult> ViewAgreement(string hashedAccountId, int? cohortId)
    {
        var request = !cohortId.HasValue
            ? new ViewEmployerAgreementRequest { AccountHashedId = hashedAccountId }
            : await _modelMapper.Map<ViewEmployerAgreementRequest>(new DetailsViewModel
            {
                AccountHashedId = hashedAccountId,
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
            return RedirectToAction("Review", new { viewModel.CohortReference, viewModel.AccountHashedId });
        }
        return RedirectToAction("Details", new { viewModel.CohortReference, viewModel.AccountHashedId });
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
        var viewModel = await _modelMapper.Map<IndexViewModel>(request);
        return View(viewModel);
    }

    [Route("add/select-provider")]
    public async Task<IActionResult> SelectProvider(SelectProviderRequest request)
    {
        var viewModel = await _modelMapper.Map<SelectProviderViewModel>(request);
        return View(viewModel);
    }

    [Route("add/select-provider")]
    [HttpPost]
    public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
    {
        try
        {
            await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));

            var confirmProviderRequest = await _modelMapper.Map<ConfirmProviderRequest>(request);
            return RedirectToAction("ConfirmProvider", confirmProviderRequest.CloneBaseValues());
        }
        catch (RestHttpClientException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(nameof(request.ProviderId), "Select a training provider");
                var returnModel = await _modelMapper.Map<SelectProviderRequest>(request);
                return RedirectToAction("SelectProvider", returnModel.CloneBaseValues());
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
    public async Task<IActionResult> ConfirmProvider(ConfirmProviderRequest request)
    {
        var model = await _modelMapper.Map<ConfirmProviderViewModel>(request);
        return View(model);
    }

    [Route("add/confirm-provider")]
    [HttpPost]
    public async Task<IActionResult> ConfirmProvider(ConfirmProviderViewModel model)
    {
        if (model.UseThisProvider.Value)
        {
            var request = await _modelMapper.Map<AssignRequest>(model);
            return RedirectToAction("assign", request.CloneBaseValues());
        }

        var returnModel = await _modelMapper.Map<SelectProviderViewModel>(model);
        return RedirectToAction("SelectProvider", returnModel);
    }

    [Route("add/assign")]
    public async Task<IActionResult> Assign(AssignRequest request)
    {
        var viewModel = await _modelMapper.Map<AssignViewModel>(request);
        return View(viewModel);
    }

    [Route("add/assign")]
    [HttpPost]
    public async Task<IActionResult> Assign(AssignViewModel model)
    {
        if (!model.ReservationId.HasValue && model.WhoIsAddingApprentices == WhoIsAddingApprentices.Employer)
        {
            var url = _linkGenerator.ReservationsLink(
                $"accounts/{model.AccountHashedId}/reservations/{model.AccountLegalEntityHashedId}/select?providerId={model.ProviderId}&transferSenderId={model.TransferSenderId}&encodedPledgeApplicationId={model.EncodedPledgeApplicationId}");
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
                    Origin = DetermineOrigin(model)
                };
                return RedirectToAction("Apprentice", "Cohort", routeValues);
            case WhoIsAddingApprentices.Provider:
                var request = await _modelMapper.Map<CreateCohortWithOtherPartyRequest>(model);
                var response = await _commitmentsApiClient.CreateCohort(request);
                return RedirectToAction("Finished", new { model.AccountHashedId, response.CohortReference });
            default:
                return RedirectToAction("Error", "Error");
        }
    }

    private static Origin DetermineOrigin(AssignViewModel source)
    {
        if (source.ReservationId.HasValue)
        {
            return Origin.Reservations;
        }

        return !string.IsNullOrWhiteSpace(source.EncodedPledgeApplicationId) ? Origin.LevyTransferMatching : Origin.Apprentices;
    }

    [HttpGet]
    [Route("add/apprentice")]
    public IActionResult Apprentice(ApprenticeRequest request)
    {
        return RedirectToAction(nameof(SelectCourse), request.CloneBaseValues());
    }

    [HttpGet]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse(ApprenticeRequest request)
    {
        var selectCourseViewModel = await _modelMapper.Map<SelectCourseViewModel>(request);
        return View("SelectCourse", selectCourseViewModel);
    }

    [HttpPost]
    [Route("add/select-course")]
    public async Task<IActionResult> SelectCourse(SelectCourseViewModel model)
    {
        var request = await _modelMapper.Map<ApprenticeRequest>(model);
        return RedirectToAction(nameof(SelectDeliveryModel), request.CloneBaseValues());
    }

    [HttpGet]
    [ActionName(nameof(SelectDeliveryModel))]
    [Route("add/select-delivery-model")]
    public async Task<IActionResult> SelectDeliveryModel(ApprenticeRequest request)
    {
        var model = await _modelMapper.Map<SelectDeliveryModelViewModel>(request);

        if (model.DeliveryModels.Length > 1)
        {
            return View("SelectDeliveryModel", model);
        }

        request.DeliveryModel = model.DeliveryModels.FirstOrDefault();
        return RedirectToAction(nameof(AddDraftApprenticeship), request.CloneBaseValues());
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

        var request = await _modelMapper.Map<ApprenticeRequest>(model);
        return RedirectToAction(nameof(AddDraftApprenticeship), request.CloneBaseValues());
    }

    [HttpGet]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeship(ApprenticeRequest request)
    {
        var model = await GetStoredApprenticeViewModelFromCache(request.CacheKey);
        if (model == null)
        {
            model = await _modelMapper.Map<ApprenticeViewModel>(request);
        }
        else
        {
            model.CourseCode = request.CourseCode;
            model.DeliveryModel = request.DeliveryModel;
        }
        return View("Apprentice", model);
    }

    [HttpPost]
    [Route("add/apprenticeship")]
    public async Task<IActionResult> AddDraftApprenticeshipOrRoute(string changeCourse, string changeDeliveryModel, ApprenticeViewModel model)
    {
        if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
        {
            await StoreApprenticeViewModelInCache(model, model.CacheKey);
            var request = await _modelMapper.Map<ApprenticeRequest>(model);

            return RedirectToAction(changeCourse == "Edit" ? nameof(SelectCourse) : nameof(SelectDeliveryModel), request.CloneBaseValues());
        }

        return await SaveDraftApprenticeship(model);
    }

    public async Task<IActionResult> SaveDraftApprenticeship(ApprenticeViewModel model)
    {
        var request = await _modelMapper.Map<CreateCohortApimRequest>(model);
        var newCohort = await _approvalsApiClient.CreateCohort(request);

        var draftApprenticeshipsResponse = await _commitmentsApiClient.GetDraftApprenticeships(newCohort.CohortId);

        var draftApprenticeship = draftApprenticeshipsResponse.DraftApprenticeships.SingleOrDefault();

        if (draftApprenticeship?.CourseCode != null)
        {
            var draftApprenticeshipHashedId = _encodingService.Encode(draftApprenticeship.Id, EncodingType.ApprenticeshipId);

            return RedirectToAction("SelectOption", "DraftApprenticeship", new { model.AccountHashedId, newCohort.CohortReference, draftApprenticeshipHashedId });
        }

        return RedirectToAction("Details", new { model.AccountHashedId, newCohort.CohortReference });
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
    [Route("review", Name = "Review")]
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
        var viewModel = await _modelMapper.Map<InformViewModel>(request);
        return View(viewModel);
    }

    [HttpGet]
    [Route("transferConnection/create")]
    public async Task<IActionResult> SelectTransferConnection(InformRequest request)
    {
        var viewModel = await _modelMapper.Map<SelectTransferConnectionViewModel>(request);

        if (viewModel.TransferConnections.Any())
        {
            return View(viewModel);
        }

        return RedirectToAction("SelectLegalEntity", new SelectLegalEntityRequest { AccountHashedId = request.AccountHashedId, transferConnectionCode = string.Empty });
    }

    [HttpPost]
    [Route("transferConnection/create")]
    public ActionResult SetTransferConnection(SelectTransferConnectionViewModel selectedTransferConnection)
    {
        var transferConnectionCode = selectedTransferConnection.TransferConnectionCode.Equals("None", StringComparison.InvariantCultureIgnoreCase)
            ? null : selectedTransferConnection.TransferConnectionCode;

        return RedirectToAction("SelectLegalEntity", new SelectLegalEntityRequest { AccountHashedId = selectedTransferConnection.AccountHashedId, transferConnectionCode = transferConnectionCode });
    }

    [HttpGet]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<IActionResult> SelectLegalEntity(SelectLegalEntityRequest request)
    {
        var response = await _modelMapper.Map<SelectLegalEntityViewModel>(request);

        if (response.LegalEntities == null || !response.LegalEntities.Any())
            throw new Exception($"No legal entities associated with account {request.AccountHashedId}");

        if (response.LegalEntities.Count() > 1)
            return View(response);

        var autoSelectLegalEntity = response.LegalEntities.First();

        var hasSignedMinimumRequiredAgreementVersion = autoSelectLegalEntity.HasSignedMinimumRequiredAgreementVersion(!string.IsNullOrWhiteSpace(request.transferConnectionCode));

        if (hasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction("SelectProvider", new BaseSelectProviderRequest
            {
                AccountHashedId = request.AccountHashedId,
                TransferSenderId = request.transferConnectionCode,
                AccountLegalEntityHashedId = autoSelectLegalEntity.AccountLegalEntityPublicHashedId,
                EncodedPledgeApplicationId = request.EncodedPledgeApplicationId
            });
        }

        var model = new LegalEntitySignedAgreementViewModel
        {
            AccountHashedId = request.AccountHashedId,
            AccountLegalEntityId = autoSelectLegalEntity.Id,
            CohortRef = request.cohortRef,
            LegalEntityName = autoSelectLegalEntity.Name,
            TransferConnectionCode = request.transferConnectionCode,
            AccountLegalEntityHashedId = autoSelectLegalEntity.AccountLegalEntityPublicHashedId
        };

        return RedirectToAction("AgreementNotSigned", model.CloneBaseValues());

    }

    [HttpPost]
    [Route("legalEntity/create")]
    [Route("add/legal-entity")]
    public async Task<ActionResult> SetLegalEntity(SelectLegalEntityViewModel selectedLegalEntity)
    {
        var response = await _modelMapper.Map<LegalEntitySignedAgreementViewModel>(selectedLegalEntity);

        if (response.HasSignedMinimumRequiredAgreementVersion)
        {
            return RedirectToAction("SelectProvider", new SelectProviderRequest
            {
                AccountHashedId = selectedLegalEntity.AccountHashedId,
                TransferSenderId = selectedLegalEntity.TransferConnectionCode,
                AccountLegalEntityHashedId = response.AccountLegalEntityHashedId,
                EncodedPledgeApplicationId = selectedLegalEntity.EncodedPledgeApplicationId
            });
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

        return RedirectToAction("AgreementNotSigned", model.CloneBaseValues());
    }

    [HttpGet]
    [Route("AgreementNotSigned")]
    public async Task<ActionResult> AgreementNotSigned(LegalEntitySignedAgreementViewModel viewModel)
    {
        var response = await _modelMapper.Map<AgreementNotSignedViewModel>(viewModel);

        return View(response);
    }

    private async Task StoreApprenticeViewModelInCache(ApprenticeViewModel model, Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            await _cacheStorageService.SaveToCache(key.Value, model, 1);
        }
    }

    private async Task<ApprenticeViewModel> GetStoredApprenticeViewModelFromCache(Guid? key)
    {
        if (key.IsNotNullOrEmpty())
        {
            return await _cacheStorageService.RetrieveFromCache<ApprenticeViewModel>(key.Value);
        }
        return null;
    }
}