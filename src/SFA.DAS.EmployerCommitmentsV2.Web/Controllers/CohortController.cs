using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
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

        [Route("{cohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort, EmployerFeature.EnhancedApproval)]
        public async Task<IActionResult> Details(DetailsRequest request)
        {
            var viewModel = await _modelMapper.Map<DetailsViewModel>(request);

            if (viewModel.WithParty != Party.Employer)
            {
                return Redirect(_linkGenerator.CohortDetails(viewModel.AccountHashedId, viewModel.CohortReference));
            }

            return View(viewModel);
        }

        [Route("{cohortReference}")]
        [DasAuthorize(CommitmentOperation.AccessCohort, EmployerFeature.EnhancedApproval)]
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel.Selection));
            }
        }

        [HttpGet]
        [Route("{cohortReference}/sent")]
        [DasAuthorize(CommitmentOperation.AccessCohort, EmployerFeature.EnhancedApproval)]
        public IActionResult Sent()
        {
            return new NotFoundResult();
        }

        [HttpGet]
        [Route("{cohortReference}/approved")]
        [DasAuthorize(CommitmentOperation.AccessCohort, EmployerFeature.EnhancedApproval)]
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
            var routeValues = new
            {
                model.AccountHashedId,
                model.AccountLegalEntityHashedId,
                model.ReservationId,
                model.StartMonthYear,
                model.CourseCode,
                model.ProviderId
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

            if (_authorizationService.IsAuthorized(EmployerFeature.EnhancedApproval))
            {
                return RedirectToAction("Details", new {model.AccountHashedId, newCohort.CohortReference });
            }

            var reviewYourCohort = _linkGenerator.CohortDetails(model.AccountHashedId, newCohort.CohortReference);
                return Redirect(reviewYourCohort);
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
                CohortReference = request.CohortReference,
                LegalEntityName = response.LegalEntityName,
                ProviderName = response.ProviderName,
                Message = response.LatestMessageCreatedByEmployer
            });
        }
    }
}