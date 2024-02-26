using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Employer.Shared.UI.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using static SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.WhyStopApprenticeshipViewModel;
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
        private readonly ILogger<ApprenticeController> _logger;
        private const string ApprenticePausedMessage = "Apprenticeship paused";
        private const string ApprenticeResumeMessage = "Apprenticeship resumed";
        private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
        private const string ApprenticeUpdated = "You have updated apprentice details";
        private const string ApprenticeEditStopDate = "New stop date confirmed";
        private const string ApprenticeEndDateUpdatedOnCompletedRecord = "New planned training finish date confirmed";
        private const string ChangesApprovedMessage = "Changes approved";
        private const string AlertDetailsWhenApproved = "An alert has been sent to the apprentice for them to re-confirm their apprenticeship details on the My apprenticeship service.";
        private const string ChangesRejectedMessage = "Changes rejected";
        private const string ChangesUndoneMessage = "Changes undone";
        private const string ViewModelForEdit = "ViewModelForEdit";
        private const string ApprenticeEndDateConfirmed = "Current planned end date confirmed ";

        public ApprenticeController(IModelMapper modelMapper, ICookieStorageService<IndexRequest> cookieStorage, ICommitmentsApiClient commitmentsApiClient, ILogger<ApprenticeController> logger)
        {
            _modelMapper = modelMapper;
            _cookieStorage = cookieStorage;
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
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
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
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

            TempData.AddFlashMessage(ApprenticeEndDateUpdatedOnCompletedRecord, ITempDataDictionaryExtensions.FlashMessageLevel.Success);

            return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
        }

        [Route("{apprenticeshipHashedId}/details/changestatus")]
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(ChangeStatusRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeStatusRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/changestatus")]
        [HttpPost]
        public IActionResult ChangeStatus(ChangeStatusRequestViewModel viewModel)
        {
            switch (viewModel.SelectedStatusChange)
            {
                case ChangeStatusType.Pause:
                    return RedirectToAction(nameof(PauseApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case ChangeStatusType.Stop:
                    var redirectToActionName = viewModel.CurrentStatus == CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart ? nameof(HasTheApprenticeBeenMadeRedundant) : nameof(WhyStopApprenticeship);
                    return RedirectToAction(redirectToActionName, new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case ChangeStatusType.Resume:
                    return RedirectToAction(nameof(ResumeApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                default:
                    return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
            }
        }

        [Route("{apprenticeshipHashedId}/change-provider", Name = RouteNames.ChangeProviderInform)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> ChangeProviderInform(ChangeProviderInformRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeProviderInformViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/change-provider/stopped-error", Name = RouteNames.ApprenticeNotStoppedError)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public IActionResult ApprenticeNotStoppedError()
        {
            return View();
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/select-provider", Name = RouteNames.EnterNewTrainingProvider)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> EnterNewTrainingProvider(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<EnterNewTrainingProviderViewModel>(request);

            if (request.Edit.GetValueOrDefault())
            {
                ViewBag.BackUrl = Url.Link(RouteNames.ConfirmDetailsAndSendRequest, request);
            }
            else
            {
                ViewBag.BackUrl = Url.Link(RouteNames.ChangeProviderInform, request);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/select-provider", Name = RouteNames.EnterNewTrainingProvider)]
        public async Task<IActionResult> EnterNewTrainingProvider(EnterNewTrainingProviderViewModel vm)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(vm);

            if (vm.Edit)
            {
                return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
            }

            return RedirectToRoute(RouteNames.WhoWillEnterTheDetails, request);
        }

        [Route("{apprenticeshipHashedId}/change-provider/who-enter-details", Name = RouteNames.WhoWillEnterTheDetails)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> WhoWillEnterTheDetails(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhoWillEnterTheDetailsViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/who-enter-details", Name = RouteNames.WhoWillEnterTheDetails)]
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
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> WhatIsTheNewStartDate(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewStartDateViewModel>(request);

            if (request.Edit.GetValueOrDefault())
            {
                ViewBag.BackUrl = Url.Link(RouteNames.ConfirmDetailsAndSendRequest, request);
            }
            else
            {
                ViewBag.BackUrl = Url.Link(RouteNames.WhoWillEnterTheDetails,
                    new
                    {
                        request.EmployerWillAdd,
                        request.ProviderName,
                        request.ProviderId
                    });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/start-date", Name = RouteNames.WhatIsTheNewStartDate)]
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
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> WhatIsTheNewEndDate(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewEndDateViewModel>(request);

            if (request.Edit.GetValueOrDefault())
            {
                ViewBag.BackUrl = Url.Link(RouteNames.ConfirmDetailsAndSendRequest, request);
            }
            else
            {
                ViewBag.BackUrl = Url.Link(RouteNames.WhatIsTheNewStartDate,
                    new
                    {
                        request.EmployerWillAdd,
                        request.ProviderId,
                        request.ProviderName,
                        request.NewStartMonth,
                        request.NewStartYear
                    });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/end-date", Name = RouteNames.WhatIsTheNewEndDate)]
        public async Task<IActionResult> WhatIsTheNewEndDate(WhatIsTheNewEndDateViewModel viewModel)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(viewModel);

            if (viewModel.Edit)
            {
                return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
            }

            return RedirectToRoute(RouteNames.WhatIsTheNewPrice, request);
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/price", Name = RouteNames.WhatIsTheNewPrice)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> WhatIsTheNewPrice(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<WhatIsTheNewPriceViewModel>(request);

            if (request.Edit.GetValueOrDefault())
            {
                ViewBag.BackUrl = Url.Link(RouteNames.ConfirmDetailsAndSendRequest, request);
            }
            else
            {
                ViewBag.BackUrl = Url.Link(RouteNames.WhatIsTheNewEndDate,
                    new
                    {
                        request.EmployerWillAdd,
                        request.ProviderId,
                        request.ProviderName,
                        request.NewStartMonth,
                        request.NewStartYear,
                        request.NewEndMonth,
                        request.NewEndYear
                    });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/price", Name = RouteNames.WhatIsTheNewPrice)]
        public async Task<IActionResult> WhatIsTheNewPrice(WhatIsTheNewPriceViewModel viewModel)
        {
            var request = await _modelMapper.Map<ChangeOfProviderRequest>(viewModel);
            return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/confirm-details", Name = RouteNames.ConfirmDetailsAndSendRequest)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> ConfirmDetailsAndSendRequestPage(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmDetailsAndSendViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/confirm-details", Name = RouteNames.ConfirmDetailsAndSendRequest)]
        public async Task<IActionResult> ConfirmDetailsAndSendRequestPage(ConfirmDetailsAndSendViewModel viewModel)
        {
            try
            {
                var apiRequest = await _modelMapper.Map<CreateChangeOfPartyRequestRequest>(viewModel);
                await _commitmentsApiClient.CreateChangeOfPartyRequest(viewModel.ApprenticeshipId, apiRequest);
                return RedirectToRoute(RouteNames.ChangeProviderRequestedConfirmation, new
                {
                    viewModel.AccountHashedId,
                    viewModel.ApprenticeshipHashedId,
                    viewModel.ProviderId,
                    viewModel.StoppedDuringCoP
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed '{nameof(ApprenticeController)}-{nameof(ConfirmDetailsAndSendRequestPage)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/cancel", Name = RouteNames.CancelChangeOfProviderRequest)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> CancelChangeOfProviderRequest(ChangeOfProviderRequest request)
        {
            var viewModel = await _modelMapper.Map<CancelChangeOfProviderRequestViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-provider/cancel", Name = RouteNames.CancelChangeOfProviderRequest)]
        public async Task<IActionResult> CancelChangeOfProviderRequest(CancelChangeOfProviderRequestViewModel viewModel)
        {
            if (viewModel.CancelRequest.Value)
            {
                return RedirectToRoute(RouteNames.ApprenticeDetail);
            }

            var request = await _modelMapper.Map<ChangeOfProviderRequest>(viewModel);

            return RedirectToRoute(RouteNames.ConfirmDetailsAndSendRequest, request);
        }

        [Route("{apprenticeshipHashedId}/change-provider/send-request", Name = RouteNames.SendRequestNewTrainingProvider)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
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
                    return RedirectToRoute(RouteNames.ChangeProviderRequestedConfirmation, new { request.AccountHashedId, request.ApprenticeshipHashedId, request.ProviderId, request.StoppedDuringCoP, ProviderAddDetails = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed '{nameof(ApprenticeController)}-{nameof(SendRequestNewTrainingProvider)}': {nameof(ex.Message)}='{ex.Message}', {nameof(ex.StackTrace)}='{ex.StackTrace}'");
                    return RedirectToAction("Error", "Error");
                }
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = request.AccountHashedId, ApprenticeshipHashedId = request.ApprenticeshipHashedId });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-provider/change-provider-requested/{providerId}", Name = RouteNames.ChangeProviderRequestedConfirmation)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> ChangeProviderRequested(ChangeProviderRequestedConfirmationRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeProviderRequestedConfirmationViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/view-changes", Name = RouteNames.ViewChanges)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> ViewChanges(ViewChangesRequest request)
        {
            var viewModel = await _modelMapper.Map<ViewChangesViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/stop", Name = RouteNames.WhenToApplyStopApprentice)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [HttpGet]
        public async Task<IActionResult> StopApprenticeship(StopRequest request)
        {
            var viewModel = await _modelMapper.Map<StopRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/stop")]
        [HttpPost]
        public IActionResult StopApprenticeship(StopRequestViewModel viewModel)
        {
            return RedirectToAction(nameof(HasTheApprenticeBeenMadeRedundant), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId, viewModel.IsCoPJourney, viewModel.StopMonth, viewModel.StopYear });
        }

        [Route("{apprenticeshipHashedId}/details/madeRedundant", Name = RouteNames.HasTheApprenticeBeenMadeRedundant)]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [HttpGet]
        public async Task<IActionResult> HasTheApprenticeBeenMadeRedundant(MadeRedundantRequest request)
        {
            var viewModel = await _modelMapper.Map<MadeRedundantViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/madeRedundant")]
        [HttpPost]
        public IActionResult HasTheApprenticeBeenMadeRedundant(MadeRedundantViewModel viewModel)
        {
            return RedirectToAction(nameof(ConfirmStop), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId, viewModel.IsCoPJourney, viewModel.StopMonth, viewModel.StopYear, viewModel.MadeRedundant });
        }

        [Route("{apprenticeshipHashedId}/details/whystopapprenticeship")]
        [HttpGet]
        public async Task<IActionResult> WhyStopApprenticeship(WhyStopApprenticeshipRequest request)
        {
            var viewModel = await _modelMapper.Map<WhyStopApprenticeshipViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/whystopapprenticeship", Name = RouteNames.WhyStopApprenticeship)]
        [HttpPost]
        public IActionResult WhyStopApprenticeship(WhyStopApprenticeshipViewModel viewModel)
        {
            switch (viewModel.SelectedStatusChange)
            {
                case StopStatusReason.LeftEmployment:
                    return RedirectToAction(nameof(StopApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case StopStatusReason.ChangeProvider:
                    return RedirectToAction(nameof(StopApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case StopStatusReason.TrainingEnded:
                    return RedirectToAction(nameof(ApprenticeshipNotEnded), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case StopStatusReason.Withdrawn:
                    return RedirectToAction(nameof(StopApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case StopStatusReason.ProviderCorrectsApprenticeRecord:
                    return RedirectToAction(nameof(StopApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                case StopStatusReason.NeverStarted:
                    return RedirectToAction(nameof(ApprenticeshipNeverStarted), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });

                default:
                    return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
            }
        }

        [Route("{apprenticeshipHashedId}/details/apprenticeshipneverstarted")]
        [HttpGet]
        public async Task<IActionResult> ApprenticeshipNeverStarted(ApprenticeshipNeverStartedRequest request)
        {
            var viewModel = await _modelMapper.Map<ApprenticeshipNeverStartedViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/apprenticeshipneverstarted", Name = RouteNames.ApprenticeshipNeverStarted)]
        [HttpPost]
        public IActionResult ApprenticeshipNeverStarted(ApprenticeshipNeverStartedViewModel viewModel)
        {
            return RedirectToAction(nameof(HasTheApprenticeBeenMadeRedundant), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId, viewModel.IsCoPJourney, viewModel.StopMonth, viewModel.StopYear });
        }

        [Route("{apprenticeshipHashedId}/details/apprenticeshipnotended")]
        [HttpGet]
        public async Task<IActionResult> ApprenticeshipNotEnded(ApprenticeshipNotEndedRequest request)
        {
            var viewModel = await _modelMapper.Map<ApprenticeshipNotEndedViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/confirmStop")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [HttpGet]
        public async Task<IActionResult> ConfirmStop(ConfirmStopRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmStopRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/confirmStop")]
        [HttpPost]
        public async Task<IActionResult> ConfirmStop(ConfirmStopRequestViewModel viewModel)
        {
            if (viewModel.StopConfirmed.HasValue && viewModel.StopConfirmed.Value)
            {
                var stopApprenticeshipRequest = await _modelMapper.Map<StopApprenticeshipRequest>(viewModel);

                await _commitmentsApiClient.StopApprenticeship(viewModel.ApprenticeshipId, stopApprenticeshipRequest, CancellationToken.None);

                if (viewModel.IsCoPJourney)
                {
                    return RedirectToAction(nameof(ApprenticeshipStoppedInform), new
                    {
                        viewModel.AccountHashedId,
                        viewModel.ApprenticeshipHashedId,
                        StoppedDuringCoP = true
                    });
                }

                TempData.AddFlashMessage(ApprenticeStoppedMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }
            return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
        }

        [Route("{apprenticeshipHashedId}/change-provider/apprenticeshipStopped")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [HttpGet]
        public IActionResult ApprenticeshipStoppedInform()
        {
            return View();
        }

        [Route("{apprenticeshipHashedId}/details/pause")]
        [HttpGet]
        public async Task<IActionResult> PauseApprenticeship(PauseRequest request)
        {
            var viewModel = await _modelMapper.Map<PauseRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/pause")]
        [HttpPost]
        public async Task<IActionResult> PauseApprenticeship(PauseRequestViewModel viewModel)
        {
            if (viewModel.PauseConfirmed.HasValue && viewModel.PauseConfirmed.Value)
            {
                var pauseRequest = await _modelMapper.Map<PauseApprenticeshipRequest>(viewModel);

                await _commitmentsApiClient.PauseApprenticeship(pauseRequest, CancellationToken.None);

                TempData.AddFlashMessage(ApprenticePausedMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
        }

        [Route("{apprenticeshipHashedId}/details/resume")]
        [HttpGet]
        public async Task<IActionResult> ResumeApprenticeship(ResumeRequest request)
        {
            var viewModel = await _modelMapper.Map<ResumeRequestViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/resume")]
        [HttpPost]
        public async Task<IActionResult> ResumeApprenticeship(ResumeRequestViewModel viewModel)
        {
            if (viewModel.ResumeConfirmed.HasValue && viewModel.ResumeConfirmed.Value)
            {
                var resumeRequest = await _modelMapper.Map<ResumeApprenticeshipRequest>(viewModel);

                await _commitmentsApiClient.ResumeApprenticeship(resumeRequest, CancellationToken.None);

                TempData.AddFlashMessage(ApprenticeResumeMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/details", Name = RouteNames.ApprenticeDetail)]
        public async Task<IActionResult> ApprenticeshipDetails(ApprenticeshipDetailsRequest request, bool showPriceChangeRejected = false, bool showPriceChangeApproved = false, bool showChangeOfPriceRequestSent = false)        {
            var viewModel = await _modelMapper.Map<ApprenticeshipDetailsRequestViewModel>(request);
            viewModel.ShowPriceChangeRejected = showPriceChangeRejected;
            viewModel.ShowPriceChangeApproved = showPriceChangeApproved;
            viewModel.ShowPriceChangeRequestSent = showChangeOfPriceRequestSent;
            return View("details", viewModel);
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/details/editstopdate", Name = "EditStopDateOption")]
        public async Task<ActionResult> EditStopDate(EditStopDateRequest request)
        {
            var viewModel = await _modelMapper.Map<EditStopDateViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/details/editstopdate", Name = "PostEditStopDate")]
        public async Task<ActionResult> UpdateApprenticeshipStopDate(EditStopDateViewModel viewModel)
        {
            var request = await _modelMapper.Map<ApprenticeshipStopDateRequest>(viewModel);

            await _commitmentsApiClient.UpdateApprenticeshipStopDate(viewModel.ApprenticeshipId, request, CancellationToken.None);

            TempData.AddFlashMessage(ApprenticeEditStopDate, ITempDataDictionaryExtensions.FlashMessageLevel.Success);

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/edit")]
        public async Task<IActionResult> EditApprenticeship(EditApprenticeshipRequest request)
        {
            var viewModel = TempData.Get<EditApprenticeshipRequestViewModel>(ViewModelForEdit) ?? await _modelMapper.Map<EditApprenticeshipRequestViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/edit", Name = RouteNames.EditApprenticeship)]
        public async Task<IActionResult> EditApprenticeship(string changeCourse, string changeDeliveryModel, EditApprenticeshipRequestViewModel viewModel)
        {
            if (changeCourse == "Edit" || changeDeliveryModel == "Edit")
            {
                TempData.Put(ViewModelForEdit, viewModel);
                return RedirectToAction(changeCourse == "Edit" ? nameof(SelectCourseForEdit) : nameof(SelectDeliveryModelForEdit), new { apprenticeshipHashedId = viewModel.HashedApprenticeshipId });
            }

            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(viewModel.ApprenticeshipId);
            // Only calculate the version if the course changes, or the start date changes and is > than the original start date.
            var triggerCalculate = viewModel.CourseCode != apprenticeship.CourseCode || apprenticeship.StartDate < viewModel.StartDate.Date.Value;
            if (triggerCalculate)
            {
                TrainingProgramme trainingProgramme;

                if (int.TryParse(viewModel.CourseCode, out var standardId))
                {
                    var standardVersionResponse = await _commitmentsApiClient.GetCalculatedTrainingProgrammeVersion(standardId, viewModel.StartDate.Date.Value);

                    trainingProgramme = standardVersionResponse.TrainingProgramme;
                }
                else
                {
                    var frameworkResponse = await _commitmentsApiClient.GetTrainingProgramme(viewModel.CourseCode);

                    trainingProgramme = frameworkResponse.TrainingProgramme;
                }

                viewModel.Version = trainingProgramme.Version;
                viewModel.HasOptions = trainingProgramme.Options.Any();
            }

            var validationRequest = await _modelMapper.Map<ValidateApprenticeshipForEditRequest>(viewModel);
            await _commitmentsApiClient.ValidateApprenticeshipForEdit(validationRequest);

            if (triggerCalculate)
            {
                viewModel.Option = null;
            }

            TempData.Put("EditApprenticeshipRequestViewModel", viewModel);

            if (viewModel.HasOptions)
            {
                return RedirectToAction("ChangeOption", new { apprenticeshipHashedId = viewModel.HashedApprenticeshipId, accountHashedId = viewModel.AccountHashedId });
            }

            return RedirectToAction("ConfirmEditApprenticeship", new { apprenticeshipHashedId = viewModel.HashedApprenticeshipId, accountHashedId = viewModel.AccountHashedId });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/edit/select-course")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> SelectCourseForEdit(EditApprenticeshipRequest request)
        {
            var draft = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>(ViewModelForEdit);
            var model = await _modelMapper.Map<SelectCourseViewModel>(draft);
            return View("SelectCourse", model);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/edit/select-course")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public IActionResult SetCourseForEdit(SelectCourseViewModel model)
        {
            if (string.IsNullOrEmpty(model.CourseCode))
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail(nameof(model.CourseCode), "You must select a training course")});
            }

            var draft = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>(ViewModelForEdit);
            draft.CourseCode = model.CourseCode;

            TempData.Put(ViewModelForEdit, draft);

            return RedirectToAction(nameof(SelectDeliveryModelForEdit));
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/edit/select-delivery-model")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public async Task<IActionResult> SelectDeliveryModelForEdit(EditApprenticeshipRequest request)
        {
            var draft = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>(ViewModelForEdit);
            var model = await _modelMapper.Map<EditApprenticeshipDeliveryModelViewModel>(draft);

            if (model.DeliveryModels.Count > 1)
            {
                return View("SelectDeliveryModel", model);
            }
            draft.DeliveryModel = (DeliveryModel) model.DeliveryModels.FirstOrDefault();
            TempData.Put(ViewModelForEdit, draft);

            return RedirectToAction("EditApprenticeship", new { request.AccountHashedId, request.ApprenticeshipHashedId });
        }

        [HttpPost]
        [Route("{ApprenticeshipHashedId}/edit/select-delivery-model")]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        public IActionResult SetDeliveryModelForEdit(EditApprenticeshipDeliveryModelViewModel model)
        {
            if (model.DeliveryModel == null)
            {
                throw new CommitmentsApiModelException(new List<ErrorDetail>
                    {new ErrorDetail("DeliveryModel", "You must select the apprenticeship delivery model")});
            }

            var draft = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>(ViewModelForEdit);
            draft.DeliveryModel = (DeliveryModel) model.DeliveryModel.Value;
            TempData.Put(ViewModelForEdit, draft);
            return RedirectToAction("EditApprenticeship");
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-version", Name = RouteNames.ChangeVersion)]
        public async Task<IActionResult> ChangeVersion(ChangeVersionRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeVersionViewModel>(request);

            // Get Edit Model if it exists to pre-select version if navigating back
            var editApprenticeViewModel = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>("EditApprenticeshipRequestViewModel");

            if (editApprenticeViewModel != null && !string.IsNullOrWhiteSpace(editApprenticeViewModel.Version))
            {
                viewModel.SelectedVersion = editApprenticeViewModel.Version;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-version")]
        public async Task<IActionResult> ChangeVersion(ChangeVersionViewModel changeVersionViewModel)
        {
            var editRequestViewModel = await _modelMapper.Map<EditApprenticeshipRequestViewModel>(changeVersionViewModel);

            TempData.Put("EditApprenticeshipRequestViewModel", editRequestViewModel);

            if (editRequestViewModel.HasOptions)
            {
                return RedirectToAction("ChangeOption", new { apprenticeshipHashedId = changeVersionViewModel.ApprenticeshipHashedId, accountHashedId = changeVersionViewModel.AccountHashedId });
            }

            return RedirectToAction("ConfirmEditApprenticeship", new { apprenticeshipHashedId = changeVersionViewModel.ApprenticeshipHashedId, accountHashedId = changeVersionViewModel.AccountHashedId });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/change-option", Name = RouteNames.ChangeOption)]
        public async Task<IActionResult> ChangeOption(ChangeOptionRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeOptionViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/change-option")]
        public async Task<IActionResult> ChangeOption(ChangeOptionViewModel viewModel)
        {
            var editViewModel = await _modelMapper.Map<EditApprenticeshipRequestViewModel>(viewModel);

            TempData.Put("EditApprenticeshipRequestViewModel", editViewModel);

            return RedirectToAction("ConfirmEditApprenticeship", new { apprenticeshipHashedId = viewModel.ApprenticeshipHashedId, accountHashedId = viewModel.AccountHashedId });
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/cancel-change-of-circumstance", Name = RouteNames.CancelInProgressChangeOfCircumstance)]
        public IActionResult CancelChangeOfCircumstance(CancelChangeOfCircumstanceRequest request)
        {
            TempData.Remove("EditApprenticeshipRequestViewModel");

            return RedirectToAction(nameof(ApprenticeshipDetails), new { request.AccountHashedId, request.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/edit/confirm")]
        public async Task<IActionResult> ConfirmEditApprenticeship()
        {
            var editApprenticeshipRequestViewModel = TempData.GetButDontRemove<EditApprenticeshipRequestViewModel>("EditApprenticeshipRequestViewModel");

            var viewModel = await _modelMapper.Map<ConfirmEditApprenticeshipViewModel>(editApprenticeshipRequestViewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/edit/confirm")]
        public async Task<IActionResult> ConfirmEditApprenticeship(ConfirmEditApprenticeshipViewModel viewModel)
        {
            if (viewModel.ConfirmChanges.Value)
            {
                var request = await _modelMapper.Map<EditApprenticeshipApiRequest>(viewModel);
                var result = await _commitmentsApiClient.EditApprenticeship(request);

                if (result.NeedReapproval)
                {
                    TempData.AddFlashMessage(ApprenticeUpdated, $"Your suggested changes have been sent to {viewModel.ProviderName} for approval.", ITempDataDictionaryExtensions.FlashMessageLevel.Success);
                }
                else
                {
                    TempData.AddFlashMessage(ApprenticeUpdated, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
                }
            }

            TempData.Remove("EditApprenticeshipRequestViewModel");

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/changes/review")]
        public async Task<IActionResult> ReviewApprenticeshipUpdates(ReviewApprenticeshipUpdatesRequest request)
        {
            var viewModel = await _modelMapper.Map<ReviewApprenticeshipUpdatesViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/changes/review")]
        public async Task<IActionResult> ReviewApprenticeshipUpdates([FromServices] IAuthenticationService authenticationService, ReviewApprenticeshipUpdatesViewModel viewModel)
        {
            if (viewModel.ApproveChanges.Value)
            {
                var request = new AcceptApprenticeshipUpdatesRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    AccountId = viewModel.AccountId,
                    UserInfo = authenticationService.UserInfo
                };

                await _commitmentsApiClient.AcceptApprenticeshipUpdates(viewModel.ApprenticeshipId, request);
                TempData.AddFlashMessageWithDetail(ChangesApprovedMessage, AlertDetailsWhenApproved, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }
            else
            {
                var request = new RejectApprenticeshipUpdatesRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    AccountId = viewModel.AccountId,
                    UserInfo = authenticationService.UserInfo
                };

                await _commitmentsApiClient.RejectApprenticeshipUpdates(viewModel.ApprenticeshipId, request);
                TempData.AddFlashMessage(ChangesRejectedMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/changes/view")]
        public async Task<IActionResult> ViewApprenticeshipUpdates(ViewApprenticeshipUpdatesRequest request)
        {
            var viewModel = await _modelMapper.Map<ViewApprenticeshipUpdatesViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/changes/view")]
        public async Task<IActionResult> ViewApprenticeshipUpdates([FromServices] IAuthenticationService authenticationService, ViewApprenticeshipUpdatesViewModel viewModel)
        {
            if (viewModel.UndoChanges.Value)
            {
                var request = new UndoApprenticeshipUpdatesRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    AccountId = viewModel.AccountId,
                    UserInfo = authenticationService.UserInfo
                };

                await _commitmentsApiClient.UndoApprenticeshipUpdates(viewModel.ApprenticeshipId, request);

                TempData.AddFlashMessage(ChangesUndoneMessage, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/changes/request")]
        public async Task<IActionResult> DataLockRequestChanges(DataLockRequestChangesRequest request)
        {
            var viewModel = await _modelMapper.Map<DataLockRequestChangesViewModel>(request);

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/changes/request")]
        public async Task<IActionResult> DataLockRequestChanges(DataLockRequestChangesViewModel viewModel)
        {
            if (viewModel.AcceptChanges.HasValue)
            {
                if (viewModel.AcceptChanges.Value)
                {
                    var request = await _modelMapper.Map<AcceptDataLocksRequestChangesRequest>(viewModel);
                    await _commitmentsApiClient.AcceptDataLockChanges(viewModel.ApprenticeshipId, request);
                }
                else
                {
                    var request = await _modelMapper.Map<RejectDataLocksRequestChangesRequest>(viewModel);
                    await _commitmentsApiClient.RejectDataLockChanges(viewModel.ApprenticeshipId, request);
                }
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
        }

        [HttpGet]
        [DasAuthorize(CommitmentOperation.AccessApprenticeship)]
        [Route("{apprenticeshipHashedId}/changes/restart")]
        public async Task<IActionResult> DataLockRequestRestart(DataLockRequestRestartRequest request)
        {
            var viewModel = await _modelMapper.Map<DataLockRequestRestartViewModel>(request);

            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/resend-email-invitation")]
        [HttpGet]
        public async Task<IActionResult> ResendEmailInvitation([FromServices] IAuthenticationService authenticationService, ResendEmailInvitationRequest request)
        {
            try
            {
                await _commitmentsApiClient.ResendApprenticeshipInvitation(request.ApprenticeshipId, new SaveDataRequest { UserInfo = authenticationService.UserInfo }, CancellationToken.None);

                TempData.AddFlashMessage("The invitation email has been resent.", null, ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }
            catch { }

            return RedirectToAction("ApprenticeshipDetails", new
            {
                AccountHashedId = request.AccountHashedId,
                ApprenticeshipHashedId = request.ApprenticeshipHashedId
            });
        }

        [Route("{apprenticeshipHashedId}/details/confirmHasNotStop")]
        [HttpGet]
        public async Task<IActionResult> ConfirmHasNotStop(ConfirmHasNotStopRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmHasNotStopViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/confirmHasValidEndDate")]
        [HttpGet]
        public async Task<IActionResult> ConfirmHasValidEndDate(ConfirmHasValidEndDateRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmHasValidEndDateViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/details/confirmHasNotStop")]
        public IActionResult ConfirmHasNotStopChanges(ConfirmHasNotStopViewModel viewModel)
        {
            if (viewModel.StopConfirmed.HasValue)
            {
                if (viewModel.StopConfirmed.Value)
                {
                    return RedirectToAction(nameof(StopApprenticeship), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
                }
                else
                {
                    return RedirectToAction(nameof(ReconfirmHasNotStop), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/details/confirmHasValidEndDate")]
        public async Task<IActionResult> ConfirmHasValidEndDateChanges(ConfirmHasValidEndDateViewModel viewModel)
        {
            if (viewModel.EndDateConfirmed.Value)
            {
                await _commitmentsApiClient.ResolveOverlappingTrainingDateRequest(new ResolveApprenticeshipOverlappingTrainingDateRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    ResolutionType = OverlappingTrainingDateRequestResolutionType.ApprenticeshipEndDateIsCorrect
                }, CancellationToken.None);

                TempData.AddFlashMessageWithDetail(ApprenticeEndDateConfirmed, viewModel.EndDate.ToGdsFormatLongMonthNameWithoutDay(), ITempDataDictionaryExtensions.FlashMessageLevel.Success);

                return RedirectToAction(nameof(ApprenticeshipDetails), new ApprenticeshipDetailsRequest { AccountHashedId = viewModel.AccountHashedId, ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId });
            }
            else
            {
                return RedirectToAction(nameof(EditEndDate), new { viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId });
            }
        }

        [Route("{apprenticeshipHashedId}/details/reconfirmHasNotStop")]
        [HttpGet]
        public async Task<IActionResult> ReconfirmHasNotStop(ReConfirmHasNotStopRequest request)
        {
            var viewModel = await _modelMapper.Map<ReconfirmHasNotStopViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/details/reconfirmHasNotStop")]
        public async Task<IActionResult> ReconfirmHasNotStopChangesAsync(ReconfirmHasNotStopViewModel viewModel)
        {
            if (viewModel.StopConfirmed.HasValue && viewModel.StopConfirmed.Value)
            {
                await _commitmentsApiClient.ResolveOverlappingTrainingDateRequest(new ResolveApprenticeshipOverlappingTrainingDateRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    ResolutionType = OverlappingTrainingDateRequestResolutionType.ApprenticeshipIsStillActive
                }, CancellationToken.None);

                TempData.AddFlashMessage($"Apprenticeship confirmed", ITempDataDictionaryExtensions.FlashMessageLevel.Success);
            }

            return RedirectToAction(nameof(ApprenticeshipDetails), new
            {
                AccountHashedId = viewModel.AccountHashedId,
                ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId
            });
        }

        [Route("{apprenticeshipHashedId}/details/confirmWhenApprenticeshipStopped")]
        [HttpGet]
        public async Task<IActionResult> ConfirmWhenApprenticeshipStopped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var viewModel = await _modelMapper.Map<ConfirmWhenApprenticeshipStoppedViewModel>(request);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{apprenticeshipHashedId}/details/confirmWhenApprenticeshipStopped")]
        public async Task<IActionResult> ConfirmWhenApprenticeshipStopped(ConfirmWhenApprenticeshipStoppedViewModel viewModel)
        {
            if (viewModel.IsCorrectStopDate.Value)
            {
                await _commitmentsApiClient.ResolveOverlappingTrainingDateRequest(new ResolveApprenticeshipOverlappingTrainingDateRequest
                {
                    ApprenticeshipId = viewModel.ApprenticeshipId,
                    ResolutionType = OverlappingTrainingDateRequestResolutionType.ApprenticeshipStopDateIsCorrect
                }, CancellationToken.None);

                TempData.AddFlashMessageWithDetail("Current stop date confirmed", viewModel.StopDate.ToGdsFormatLongMonthNameWithoutDay(), ITempDataDictionaryExtensions.FlashMessageLevel.Success);

                return RedirectToAction(nameof(ApprenticeshipDetails), new
                {
                    AccountHashedId = viewModel.AccountHashedId,
                    ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId
                });
            }
            else
            {
                return RedirectToAction(nameof(EditStopDate), new
                {
                    AccountHashedId = viewModel.AccountHashedId,
                    ApprenticeshipHashedId = viewModel.ApprenticeshipHashedId
                });
            }
        }
    }
}