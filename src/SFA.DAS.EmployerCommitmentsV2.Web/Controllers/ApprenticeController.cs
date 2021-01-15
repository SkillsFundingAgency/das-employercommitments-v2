using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Employer.Shared.UI.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
using EditEndDateRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.EditEndDateRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountHashedId}/apprentices")]
    [SetNavigationSection(NavigationSection.ApprenticesHome)]
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    public class ApprenticeController : Controller
    {
        private readonly IModelMapper _modelMapper;
        private readonly ICookieStorageService<IndexRequest> _cookieStorage;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ILogger<ApprenticeController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public ApprenticeController(IModelMapper modelMapper, ICookieStorageService<IndexRequest> cookieStorage, ICommitmentsApiClient commitmentsApiClient, ILinkGenerator linkGenerator, ILogger<ApprenticeController> logger, IAuthorizationService authorizationService)
        {
            _modelMapper = modelMapper;
            _cookieStorage = cookieStorage;
            _commitmentsApiClient = commitmentsApiClient;
            _linkGenerator = linkGenerator;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        [Route("", Name = RouteNames.ApprenticesIndex)]
        public async Task<IActionResult> Index(IndexRequest request)
        {
            IndexRequest savedRequest = null;

            if (request.FromSearch)
            {
                savedRequest = _cookieStorage.Get(CookieNames.ManageApprentices);

                if (savedRequest != null)
                {
                    request = savedRequest;
                }
            }

            if (savedRequest == null)
            {
                _cookieStorage.Update(CookieNames.ManageApprentices, request);
            }

            var viewModel = await _modelMapper.Map<IndexViewModel>(request);
            viewModel.SortedByHeader();

            return View(viewModel);
        }

        [Route("download", Name = RouteNames.ApprenticesDownload)]
        public async Task<IActionResult> Download(DownloadRequest request)
        {
            var downloadViewModel = await _modelMapper.Map<DownloadViewModel>(request);

            return File(downloadViewModel.Content, downloadViewModel.ContentType, downloadViewModel.Name);
        }

        [Route("{apprenticeshipHashedId}/details/editenddate", Name = RouteNames.ApprenticeEditEndDate)]
        public async Task<IActionResult> EditEndDate(EditEndDateRequest request)
        {
            var viewModel = await _modelMapper.Map<EditEndDateViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/editenddate", Name = RouteNames.ApprenticeEditEndDate)]
        [HttpPost]
        public async Task<IActionResult> EditEndDate(EditEndDateViewModel viewModel)
        {
            var request = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.EditEndDateRequest>(viewModel);
            await _commitmentsApiClient.UpdateEndDateOfCompletedRecord(request, CancellationToken.None);
            var url = _linkGenerator.ApprenticeDetails(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId);
            return Redirect(url);
        }

        [Route("{apprenticeshipHashedId}/details/changestatus")]
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(ChangeStatusRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeStatusRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/changestatus")]
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [HttpPost]
        public IActionResult ChangeStatus(ChangeStatusRequestViewModel viewModel)
        {
            switch (viewModel.SelectedStatusChange)
            {
                case ChangeStatusType.Pause:
                    return RedirectToAction(nameof(PauseApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
                case ChangeStatusType.Stop:
                    return Redirect(_linkGenerator.WhenToApplyStopApprentice(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId));
                case ChangeStatusType.Resume:
                    return RedirectToAction(nameof(ResumeApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
                default:
                    return Redirect(_linkGenerator.ApprenticeDetails(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId));
            }
        }

        [Route("{apprenticeshipHashedId}/change-provider", Name = RouteNames.ChangeProviderInform)]
        public async Task<IActionResult> ChangeProviderInform(ChangeProviderInformRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeProviderInformViewModel>(request);

            return View(viewModel);
        }

        // Placeholder for CON-2516 - url not specified yet
        [Route("{apprenticeshipHashedId}/change-provider/stopped-error", Name = RouteNames.ApprenticeNotStoppedError)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public IActionResult ApprenticeNotStoppedError()
        {
            return View();
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/select-provider", Name = RouteNames.EnterNewTrainingProvider)]
        public async Task<IActionResult> EnterNewTrainingProvider(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<EnterNewTrainingProviderViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/select-provider", Name = RouteNames.EnterNewTrainingProvider)]
        public async Task<IActionResult> EnterNewTrainingProvider(EnterNewTrainingProviderViewModel vm)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(vm);

            if (_authorizationService.IsAuthorized(EmployerFeature.ChangeOfProvider))
            {
                if (vm.Edit)
                {
                    return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
                }

                return RedirectToRoute(RouteNames.WhoWillEnterTheDetails, request);
            }

            return RedirectToRoute(RouteNames.SendRequestNewTrainingProvider, request);
        }

        [Route("{apprenticeshipHashedId}/change-provider/who-enter-details", Name = RouteNames.WhoWillEnterTheDetails)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhoWillEnterTheDetails(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhoWillEnterTheDetailsViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/who-enter-details", Name = RouteNames.WhoWillEnterTheDetails)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhoWillEnterTheDetails(WhoWillEnterTheDetailsViewModel vm)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(vm);

            if (vm.EmployerWillAdd == true)
            {
                return RedirectToRoute(RouteNames.WhatIsTheNewStartDate, request);
            }
            else
            {
                return RedirectToRoute(RouteNames.SendRequestNewTrainingProvider, request);
            }
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/start-date", Name = RouteNames.WhatIsTheNewStartDate)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhatIsTheNewStartDate(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewStartDateViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/start-date", Name = RouteNames.WhatIsTheNewStartDate)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhatIsTheNewStartDate(WhatIsTheNewStartDateViewModel vm)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(vm);
          
            if (vm.Edit)
            {
                return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
            }

            return RedirectToRoute(RouteNames.WhatIsTheNewEndDate, request);
        }


        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/end-date", Name = RouteNames.WhatIsTheNewEndDate)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhatIsTheNewEndDate(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewEndDateViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/end-date", Name = RouteNames.WhatIsTheNewEndDate)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public IActionResult WhatIsTheNewEndDate(WhatIsTheNewEndDateViewModel viewModel)
        {
            if (viewModel.Edit)
            {
                return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, 
                    new { 
                        viewModel.ProviderName,
                        viewModel.AccountHashedId,
                        viewModel.ApprenticeshipHashedId,
                        viewModel.ProviderId,
                        viewModel.NewStartMonth,
                        viewModel.NewStartYear,
                        viewModel.NewEndMonth,
                        viewModel.NewEndYear,
                        viewModel.NewPrice });
            }

            return RedirectToRoute(RouteNames.WhatIsTheNewPrice,
                new
                {
                    viewModel.ProviderName,
                    viewModel.AccountHashedId,
                    viewModel.ApprenticeshipHashedId,
                    viewModel.ProviderId,
                    viewModel.NewStartMonth,
                    viewModel.NewStartYear,
                    viewModel.NewEndMonth,
                    viewModel.NewEndYear
                });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/price", Name = RouteNames.WhatIsTheNewPrice)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> WhatIsTheNewPrice(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewPriceViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/price", Name = RouteNames.WhatIsTheNewPrice)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public IActionResult WhatIsTheNewPrice(WhatIsTheNewPriceViewModel viewModel)
        {
            return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, new { viewModel.ProviderName, viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId, viewModel.ProviderId, viewModel.NewStartMonth, viewModel.NewStartYear, viewModel.NewEndMonth, viewModel.NewEndYear, viewModel.NewPrice });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/confirm-details", Name = RouteNames.ConfirmDetailsAndSendRequest)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> ConfirmDetailsAndSendRequestPage(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmDetailsAndSendViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/confirm-details", Name = RouteNames.ConfirmDetailsAndSendRequest)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> ConfirmDetailsAndSendRequestPage(ConfirmDetailsAndSendViewModel viewModel)
        {
            try
            {
                var apiRequest = await _modelMapper.Map<CreateChangeOfPartyRequestRequest>(viewModel);
                await _commitmentsApiClient.CreateChangeOfPartyRequest(viewModel.ApprenticeshipId, apiRequest);
                return RedirectToRoute(RouteNames.ChangeProviderRequestedConfirmation, new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId, viewModel.ProviderId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{nameof(ApprenticeController)}-{nameof(ConfirmDetailsAndSendRequestPage)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
                return RedirectToAction("Error", "Error");
            }
        }

        [Route("{apprenticeshipHashedId}/change-provider/send-request", Name = RouteNames.SendRequestNewTrainingProvider)]
        public async Task<IActionResult> SendRequestNewTrainingProvider(SendNewTrainingProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<SendNewTrainingProviderViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/send-request", Name = RouteNames.SendRequestNewTrainingProvider)]
        public async Task<IActionResult> SendRequestNewTrainingProvider(SendNewTrainingProviderViewModel request)
        {
            if (request.Confirm.Value)
            {
                try
                {
                    var apiRequest = await _modelMapper.Map<CreateChangeOfPartyRequestRequest>(request);
                    await _commitmentsApiClient.CreateChangeOfPartyRequest(request.ApprenticeshipId, apiRequest);
                    return RedirectToRoute(RouteNames.ChangeProviderRequestedConfirmation, new { request.AccountHashedId, request.ApprenticeshipHashedId, request.ProviderId });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed '{nameof(ApprenticeController)}-{nameof(SendRequestNewTrainingProvider)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
                    return RedirectToAction("Error", "Error");
                }
            }

            return Redirect(_linkGenerator.ApprenticeDetails(request.AccountHashedId, request.ApprenticeshipHashedId));
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/change-provider-requested/{providerId}", Name = RouteNames.ChangeProviderRequestedConfirmation)]
        public async Task<IActionResult> ChangeProviderRequested(ChangeProviderRequestedConfirmationRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeProviderRequestedConfirmationViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/view-changes", Name = RouteNames.ViewChanges)]
        public async Task<IActionResult> ViewChanges(ViewChangesRequest request)
        {
            var viewModel = await _modelMapper.Map<ViewChangesViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/pause")]
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [HttpGet]
        public async Task<IActionResult> PauseApprenticeship(PauseRequest request)
        {
            var viewModel = await _modelMapper.Map<PauseRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/pause")]
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [HttpPost]
        public async Task<IActionResult> PauseApprenticeship(PauseRequestViewModel viewModel)
        {
            if (viewModel.PauseConfirmed.HasValue && viewModel.PauseConfirmed.Value)
            {
                var pauseRequest = new PauseApprenticeshipRequest { ApprenticeshipId = viewModel.ApprenticeshipId };

                await _commitmentsApiClient.PauseApprenticeship(pauseRequest, CancellationToken.None);
            }

            return Redirect(_linkGenerator.ApprenticeDetails(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId));
        }

        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [Route("{apprenticeshipHashedId}/details/resume")]
        [HttpGet]
        public async Task<IActionResult> ResumeApprenticeship(ResumeRequest request)
        {
            var viewModel = await _modelMapper.Map<ResumeRequestViewModel>(request);
            return View(viewModel);
        }

        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        [Route("{apprenticeshipHashedId}/details/resume")]
        [HttpPost]
        public async Task<IActionResult> ResumeApprenticeship(ResumeRequestViewModel viewModel)
        {
            if (viewModel.ResumeConfirmed.HasValue && viewModel.ResumeConfirmed.Value)
            {
                var resumeRequest = new ResumeApprenticeshipRequest { ApprenticeshipId = viewModel.ApprenticeshipId };

                await _commitmentsApiClient.ResumeApprenticeship(resumeRequest, CancellationToken.None);
            }

            return Redirect(_linkGenerator.ApprenticeDetails(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId));
        }
    }
}