using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{AccountHashedId}/unapproved/{cohortReference}/apprentices")]
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly IMapper<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel> _editDraftApprenticeshipDetailsToViewModelMapper;
        private readonly IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest> _updateDraftApprenticeshipRequestMapper;
        private readonly ILinkGenerator _linkGenerator;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public DraftApprenticeshipController(
            ICommitmentsService commitmentsService,
            IMapper<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel> editDraftApprenticeshipDetailsToViewModelMapper,
            IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest> updateDraftApprenticeshipRequestMapper,
            ILinkGenerator linkGenerator,
            ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsService = commitmentsService;
            _editDraftApprenticeshipDetailsToViewModelMapper = editDraftApprenticeshipDetailsToViewModelMapper;
            _updateDraftApprenticeshipRequestMapper = updateDraftApprenticeshipRequestMapper;
            _linkGenerator = linkGenerator;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        [HttpGet]
        [Route("{DraftApprenticeshipHashedId}/edit")]
        public async Task<IActionResult> EditDraftApprenticeship(EditDraftApprenticeshipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var editModel =
                    await _commitmentsService.GetDraftApprenticeshipForCohort(request.CohortId,
                        request.DraftApprenticeshipId);
                var model = _editDraftApprenticeshipDetailsToViewModelMapper.Map(editModel);

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
            if (!ModelState.IsValid)
            {
                await AddProviderNameAndCoursesToModel(model);
                return View(model);
            }

            try
            {
                var updateRequest = _updateDraftApprenticeshipRequestMapper.Map(model);
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