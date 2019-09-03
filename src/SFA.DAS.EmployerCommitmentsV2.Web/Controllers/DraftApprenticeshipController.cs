using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerUrlHelper;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [DasAuthorize(CommitmentOperation.AccessCohort, EmployerFeature.EmployerCommitmentsV2, EmployerUserRole.OwnerOrTransactor)]
    [Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IModelMapper _modelMapper;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public DraftApprenticeshipController(
            ICommitmentsService commitmentsService,
            ILinkGenerator linkGenerator,
            ITrainingProgrammeApiClient trainingProgrammeApiClient,
            IModelMapper modelMapper)
        {
            _commitmentsService = commitmentsService;
            _linkGenerator = linkGenerator;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
            _modelMapper = modelMapper;
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipRequest request)
        {
            try
            {
                var model = await _modelMapper.Map<AddDraftApprenticeshipViewModel>(request);
                return View(model);
            }
            catch (CohortEmployerUpdateDeniedException)
            {
                return Redirect(_linkGenerator.CohortDetails(request.AccountHashedId, request.CohortReference));
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
                var addDraftApprenticeshipRequest = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(model);
                await _commitmentsService.AddDraftApprenticeshipToCohort(model.CohortId.Value, addDraftApprenticeshipRequest);
                return Redirect(_linkGenerator.CohortDetails(model.AccountHashedId, model.CohortReference));
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        public async Task<IActionResult> EditDraftApprenticeship(EditDraftApprenticeshipRequest request)
        {
            try
            {
                var editModel =
                    await _commitmentsService.GetDraftApprenticeshipForCohort(request.CohortId,
                        request.DraftApprenticeshipId);

                var model = await _modelMapper.Map<EditDraftApprenticeshipViewModel>(editModel);

                model.AccountHashedId = request.AccountHashedId;
                
                await AddProviderNameAndCoursesToModel(model);

                return View(model);
            }
            catch (CohortEmployerUpdateDeniedException)
            {
                return Redirect(_linkGenerator.ViewApprentice(request.AccountHashedId, request.CohortReference, request.DraftApprenticeshipHashedId));
            }
        }

        [HttpPost]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        public async Task<IActionResult> EditDraftApprenticeship(EditDraftApprenticeshipViewModel model)
        {
            try
            {
                var updateRequest = await _modelMapper.Map<UpdateDraftApprenticeshipRequest>(model);
                await _commitmentsService.UpdateDraftApprenticeship(model.CohortId.Value, model.DraftApprenticeshipId, updateRequest);

                var reviewYourCohort = _linkGenerator.CohortDetails(model.AccountHashedId, model.CohortReference);
                return Redirect(reviewYourCohort);
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                await AddProviderNameAndCoursesToModel(model);
                return View(model);
            }
        }

        private async Task<CohortDetails> GetCohortDetails(long cohortId)
        {
            var cohort = await _commitmentsService.GetCohortDetail(cohortId);

            if (cohort.WithParty != Party.Employer)
                throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

            return cohort;
        }

        private async Task AddProviderNameAndCoursesToModel(DraftApprenticeshipViewModel model)
        {
            var cohort = await GetCohortDetails(model.CohortId.Value);
            var courses = await GetCourses(!cohort.IsFundedByTransfer);

            model.ProviderName = cohort.ProviderName;
            model.Courses = courses;
        }

        private Task<IReadOnlyList<ITrainingProgramme>> GetCourses(bool includeFrameworks)
        {
            if (includeFrameworks)
            {
                return _trainingProgrammeApiClient.GetAllTrainingProgrammes();
            }
            return _trainingProgrammeApiClient.GetStandardTrainingProgrammes();
        }
    }
}