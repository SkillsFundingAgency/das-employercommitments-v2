using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Commitments.Shared.Extensions;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.EmployerCommitmentsV2.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    
    [Route("accounts/{hashedAccountId}/apprentices/")]
    public class DraftApprenticeshipController : Controller
    {
        private readonly ICommitmentsService _employerCommitmentsService;
        private readonly IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest> _addDraftApprenticeshipToCohortRequestMapper;
        private readonly ILinkGenerator _urlHelper;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public DraftApprenticeshipController(
            ICommitmentsService employerCommitmentsService,
            IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest> addDraftApprenticeshipToCohortRequestMapper,
            ILinkGenerator urlHelper,
            ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _employerCommitmentsService = employerCommitmentsService;
            _addDraftApprenticeshipToCohortRequestMapper = addDraftApprenticeshipToCohortRequestMapper;
            _urlHelper = urlHelper;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(ReservationsAddDraftApprenticeshipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new AddDraftApprenticeshipViewModel
            {
                CohortReference = request.CohortReference,
                CohortId = request.CohortId,
                StartDate = new MonthYearModel(request.StartMonthYear),
                ReservationId = request.ReservationId,
                CourseCode = request.CourseCode
            };

            await AddLegalEntityAndCoursesToModel(model);

            return View(model);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await AddLegalEntityAndCoursesToModel(model);
                return View(model);
            }

            var request = _addDraftApprenticeshipToCohortRequestMapper.Map(model);
            request.UserId = User.Upn();

            try
            {
                await _employerCommitmentsService.AddDraftApprenticeshipToCohort(model.CohortId.Value, request);
                return Redirect("add");
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);
                await AddLegalEntityAndCoursesToModel(model);
                return View(model);
            }
        }

        private async Task AddLegalEntityAndCoursesToModel(DraftApprenticeshipViewModel model)
        {
            var cohortDetail = await _employerCommitmentsService.GetCohortDetail(model.CohortId.Value);
            var courses = await GetCourses(cohortDetail);
            model.Courses = courses;
        }

        private Task<IReadOnlyList<ITrainingProgramme>> GetCourses(CohortDetails cohortDetails)
        {
            if (cohortDetails.IsFundedByTransfer)
            {
                return _trainingProgrammeApiClient.GetStandardTrainingProgrammes();
            }

            return _trainingProgrammeApiClient.GetAllTrainingProgrammes();
        }
    }
}