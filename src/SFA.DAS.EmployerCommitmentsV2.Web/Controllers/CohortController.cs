using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved")]
    public class CohortController : Controller
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<CohortController> _logger;
        private readonly ILinkGenerator _linkGenerator;
        private readonly IModelMapper _modelMapper;
        private readonly IAuthorizationService _authorizationService;

        public CohortController(
            ICommitmentsApiClient commitmentsApiClient,
            ILogger<CohortController> logger,
            ILinkGenerator linkGenerator,
            IModelMapper modelMapper,
            IAuthorizationService authorizationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _modelMapper = modelMapper;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Cohorts(CohortsByAccountRequest request)
        {
            var model = await _modelMapper.Map<CohortsViewModel>(request);
            return View(model);
        }

        [Route("{cohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public async Task<IActionResult> Details(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<DetailsViewModel>(request);

            return View(viewModel);
        }

        [Route("{cohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        [HttpPost]
        public async Task<IActionResult> Details(DetailsViewModel viewModel)
        {
            switch (viewModel.Selection)
            {
                case CohortDetailsOptions.Send:
                {
                    var request = await _modelMapper.Map<SendCohortRequest>(viewModel);
                    await _commitmentsApiClient.SendCohort(viewModel.CohortId, request);
                    return RedirectToAction("Sent", new { viewModel.CohortReference, viewModel.AccountHashedId});
                }
                case CohortDetailsOptions.Approve:
                {
                    var request = await _modelMapper.Map<ApproveCohortRequest>(viewModel);
                    await _commitmentsApiClient.ApproveCohort(viewModel.CohortId, request);
                    return RedirectToAction("Approved", new { viewModel.CohortReference, viewModel.AccountHashedId });
                }
                case CohortDetailsOptions.ViewEmployerAgreement:
                {
                    var request = await _modelMapper.Map<ViewEmployerAgreementRequest>(viewModel);
                    if (request.AgreementHashedId == null)
                    {
                        return Redirect(_linkGenerator.AccountsLink($"accounts/{request.AccountHashedId}/agreements/"));
                    }
                    return Redirect(_linkGenerator.AccountsLink(
                    $"accounts/{request.AccountHashedId}/agreements/{request.AgreementHashedId}/about-your-agreement"));
                }
                case CohortDetailsOptions.Homepage:
                {
                    return Redirect(_linkGenerator.AccountsLink($"accounts/{viewModel.AccountHashedId}/teams"));
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel.Selection));
            }
        }

        [Route("{cohortReference}/delete")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public async Task<IActionResult> ConfirmDelete(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmDeleteViewModel>(request);
            return View(viewModel);
        }

        [Route("{cohortReference}/delete")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        [HttpPost]
        public async Task<IActionResult> Delete([FromServices] IAuthenticationService authenticationService, ConfirmDeleteViewModel viewModel)
        {
            if(viewModel.ConfirmDeletion == true)
            { 
                await _commitmentsApiClient.DeleteCohort(viewModel.CohortId, authenticationService.UserInfo, CancellationToken.None);
                return RedirectToAction("Cohorts", new {viewModel.AccountHashedId});
            }
            return RedirectToAction("Details", new { viewModel.CohortReference, viewModel.AccountHashedId });
        }

        [HttpGet]
        [Route("{cohortReference}/sent")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public async Task<IActionResult> Sent(SentRequest request)
        {
            var viewModel = await _modelMapper.Map<SentViewModel>(request);
            return View(viewModel);
        }

        [HttpGet]
        [Route("{cohortReference}/approved")]
        [DasAuthorize(CommitmentOperation.AccessCohort)]
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
                return RedirectToAction("ConfirmProvider", confirmProviderRequest);
            }
            catch (RestHttpClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(nameof(request.ProviderId), "Check UK Provider Reference Number");
                    var returnModel = await _modelMapper.Map<SelectProviderRequest>(request);
                    return RedirectToAction("SelectProvider", returnModel);
                }

                _logger.LogError(
                    $"Failed '{nameof(CohortController)}-{nameof(SelectProvider)}': {nameof(ex.StatusCode)}='{ex.StatusCode}', {nameof(ex.ReasonPhrase)}='{ex.ReasonPhrase}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Failed '{nameof(CohortController)}-{nameof(SelectProvider)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
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
        public async Task<IActionResult> ConfirmProvider(ConfirmProviderViewModel request)
        {
            if (request.UseThisProvider.Value)
            {
                var model = await _modelMapper.Map<AssignRequest>(request);
                return RedirectToAction("assign", model);
            }

            var returnModel = await _modelMapper.Map<SelectProviderViewModel>(request);
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
        public IActionResult Assign(AssignViewModel model)
        {
            if (!model.ReservationId.HasValue && model.WhoIsAddingApprentices == WhoIsAddingApprentices.Employer)
            {
                var url = _linkGenerator.ReservationsLink(
                    $"accounts/{model.AccountHashedId}/reservations/{model.AccountLegalEntityHashedId}/select?providerId={model.ProviderId}&transferSenderId={model.TransferSenderId}");
                return Redirect(url);
            }

            var routeValues = new
            {
                model.AccountHashedId,
                model.AccountLegalEntityHashedId,
                model.ReservationId,
                model.StartMonthYear,
                model.CourseCode,
                model.ProviderId,
                model.TransferSenderId
            };

            switch (model.WhoIsAddingApprentices)
            {
                case WhoIsAddingApprentices.Employer:
                    return RedirectToAction("Apprentice", "Cohort", routeValues);
                case WhoIsAddingApprentices.Provider:
                    return RedirectToAction("Message", routeValues);
                default:
                    return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        [Route("add/apprentice")]
        public async Task<IActionResult> Apprentice(ApprenticeRequest request)
        {
            var model = await _modelMapper.Map<ApprenticeViewModel>(request);
            return View(model);
        }

        [HttpPost]
        [Route("add/apprentice")]
        public async Task<IActionResult> Apprentice(ApprenticeViewModel model)
        {
            var request = await _modelMapper.Map<CreateCohortRequest>(model);
            var newCohort = await _commitmentsApiClient.CreateCohort(request);

            return RedirectToAction("Details", new { model.AccountHashedId, newCohort.CohortReference });
        }

        [Route("add/message")]
        public async Task<IActionResult> Message(MessageRequest request)
        {
            var model = await _modelMapper.Map<MessageViewModel>(request);
            return View(model);
        }

        [HttpPost]
        [Route("add/message")]
        public async Task<IActionResult> Message(MessageViewModel model)
        {
            var request = await _modelMapper.Map<CreateCohortWithOtherPartyRequest>(model);
            var response = await _commitmentsApiClient.CreateCohort(request);
            return RedirectToAction("Finished", new { model.AccountHashedId, response.CohortReference });
        }

        [DasAuthorize(CommitmentOperation.AccessCohort)]
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
        [Route("review")]
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
    }
}