using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class ConfirmDeleteViewModelMapper : IMapper<DetailsRequest, ConfirmDeleteViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public ConfirmDeleteViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<ConfirmDeleteViewModel> Map(DetailsRequest source)
        {
            var cohortTask = _commitmentsApiClient.GetCohort(source.CohortId);
            var draftApprenticeshipsTask = _commitmentsApiClient.GetDraftApprenticeships(source.CohortId);

            await Task.WhenAll(cohortTask, draftApprenticeshipsTask);

            var cohort = await cohortTask;
            var draftApprenticeships = (await draftApprenticeshipsTask).DraftApprenticeships;

            return new ConfirmDeleteViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                ProviderName = cohort.ProviderName,
                LegalEntityName = cohort.LegalEntityName,
                Courses = GroupCourses(draftApprenticeships)
            };
        }

        private IReadOnlyCollection<ConfirmDeleteViewModel.CourseGroupingModel> GroupCourses(IEnumerable<DraftApprenticeshipDto> draftApprenticeships)
        {
            var groupedByCourse = draftApprenticeships
                .GroupBy(a => new { a.CourseCode, a.CourseName })
                .Select(course => new ConfirmDeleteViewModel.CourseGroupingModel
                {
                    CourseName = course.Key.CourseName,
                    NumberOfDraftApprenticeships = course.Count()
                })
            .OrderBy(c => c.CourseName)
                .ToList();

            return groupedByCourse;
        }
    }
}