using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(EmployerFeature.EmployerCommitmentsV2, EmployerUserRole.OwnerOrTransactor)]
    [Route("{accountHashedId}/unapproved")]
    public class CohortController : Controller
    {
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ICommitmentsService _employerCommitmentsService;
        private readonly ILogger<CohortController> _logger;
        private readonly ILinkGenerator _linkGenerator;
        private readonly IModelMapper _modelMapper;

        public CohortController(
            ICommitmentsApiClient commitmentsApiClient,
            ILogger<CohortController> logger,
            ICommitmentsService employerCommitmentsService,
            ITrainingProgrammeApiClient trainingProgrammeApiClient,
            ILinkGenerator linkGenerator,
            IModelMapper modelMapper)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
            _employerCommitmentsService = employerCommitmentsService;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
            _linkGenerator = linkGenerator;
            _modelMapper = modelMapper;
        }

        [Route("add")]
        public IActionResult Index(IndexRequest request)
        {
            var viewModel = _modelMapper.Map<IndexViewModel>(request);
            return View(viewModel);
        }

        [Route("add/select-provider")]
        public IActionResult SelectProvider(SelectProviderRequest request)
        {
            var viewModel = _modelMapper.Map<SelectProviderViewModel>(request);
            return View(viewModel);
        }

        [Route("add/select-provider")]
        [HttpPost]
        public async Task<IActionResult> SelectProvider(SelectProviderViewModel request)
        {
            try
            {
                await _commitmentsApiClient.GetProvider(long.Parse(request.ProviderId));

                var confirmProviderRequest = _modelMapper.Map<ConfirmProviderRequest>(request);
                return RedirectToAction("ConfirmProvider", confirmProviderRequest);
            }
            catch (RestHttpClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(nameof(request.ProviderId), "Check UK Provider Reference Number");
                    return View(request);
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
            var response = await _commitmentsApiClient.GetProvider(request.ProviderId);

            var model = _modelMapper.Map<ConfirmProviderViewModel>(request);
            model.ProviderId = response.ProviderId; //todo: can we move/remove these?
            model.ProviderName = response.Name;

            return View(model);
        }

        [Route("add/confirm-provider")]
        [HttpPost]
        public IActionResult ConfirmProvider(ConfirmProviderViewModel request)
        {
            if (request.UseThisProvider.Value)
            {
                var model = _modelMapper.Map<AssignRequest>(request);
                return RedirectToAction("assign", model);
            }

            var returnModel = _modelMapper.Map<SelectProviderViewModel>(request);
            return RedirectToAction("SelectProvider", returnModel);
        }

        [Route("add/assign")]
        public IActionResult Assign(AssignRequest request)
        {
            var viewModel = _modelMapper.Map<AssignViewModel>(request);
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
                    return RedirectToAction("AddDraftApprenticeship", "Cohort", routeValues);
                case WhoIsAddingApprentices.Provider:
                    return RedirectToAction("Message", routeValues);
                default:
                    return RedirectToAction("Error", "Error");
            }
        }


        [HttpGet]
        [Route("add/apprentice")]
        public async Task<IActionResult> AddDraftApprenticeship(CreateCohortWithDraftApprenticeshipRequest request)
        {
            var model = new AddDraftApprenticeshipViewModel
            {
                AccountLegalEntityId = request.AccountLegalEntityId,
                AccountLegalEntityHashedId = request.AccountLegalEntityHashedId,
                StartDate = new MonthYearModel(request.StartMonthYear),
                ReservationId = request.ReservationId,
                CourseCode = request.CourseCode,
                ProviderId = (int)request.ProviderId
            };

            await AddCoursesAndProviderNameToModel(model);

            return View(model);
        }

        [HttpPost]
        [Route("add/apprentice")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
            var request = _modelMapper.Map<CreateCohortRequest>(model);
            
            try
            {
                var newCohort = await _employerCommitmentsService.CreateCohort(request);
                var reviewYourCohort = _linkGenerator.CohortDetails(model.AccountHashedId, newCohort.CohortReference);
                return Redirect(reviewYourCohort);
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                await AddCoursesAndProviderNameToModel(model);
                return View(model);
            }
        }

        [Route("add/message")]
        public async Task<IActionResult> Message(MessageRequest request)
        {
            var messageModel = new MessageViewModel
            {
                AccountHashedId = request.AccountHashedId,
                AccountLegalEntityHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                ProviderId = request.ProviderId,
                StartMonthYear = request.StartMonthYear,
                CourseCode = request.CourseCode,
                ReservationId = request.ReservationId
            };
            messageModel.ProviderName = await GetProviderName(messageModel.ProviderId);

            return View(messageModel);
        }

        [HttpPost]
        [Route("add/message")]
        public async Task<IActionResult> Message(MessageViewModel model)
        {
            try
            {
                var request = _modelMapper.Map<CreateCohortWithOtherPartyRequest>(model);
                var response = await _commitmentsApiClient.CreateCohort(request);
                return RedirectToAction("Finished", new { model.AccountHashedId, response.CohortReference });
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                model.ProviderName = await GetProviderName(model.ProviderId);
                return View(model);
            }
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

        private async Task<string> GetProviderName(long providerId)
        {
            return (await _commitmentsApiClient.GetProvider(providerId)).Name;
        }


        private async Task AddCoursesAndProviderNameToModel(DraftApprenticeshipViewModel model)
        {
            var courses = await GetCourses();
            model.Courses = courses;

            var providerName = await GetProviderName(model.ProviderId);
            model.ProviderName = providerName;
        }

        private Task<IReadOnlyList<ITrainingProgramme>> GetCourses()
        {
            return _trainingProgrammeApiClient.GetAllTrainingProgrammes();
        }

    }
}