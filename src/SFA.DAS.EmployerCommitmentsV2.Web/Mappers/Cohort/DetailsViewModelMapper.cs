using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DetailsViewModelMapper : IMapper<DetailsRequest, DetailsViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly ITrainingProgrammeApiClient _trainingProgrammeApiClient;

        public DetailsViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, ITrainingProgrammeApiClient trainingProgrammeApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
        }

        public async Task<DetailsViewModel> Map(DetailsRequest source)
        {
            var cohortTask = _commitmentsApiClient.GetCohort(source.CohortId);
            var draftApprenticeshipsTask = _commitmentsApiClient.GetDraftApprenticeships(source.CohortId);

            await Task.WhenAll(cohortTask, draftApprenticeshipsTask);

            var cohort = cohortTask.Result;
            var draftApprenticeships = draftApprenticeshipsTask.Result.DraftApprenticeships;

            return new DetailsViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                WithParty = cohort.WithParty,
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                Message = cohort.LatestMessageCreatedByProvider,
                Courses = await GroupCourses(draftApprenticeships.ToList())
            };
        }

        private async Task<IReadOnlyCollection<DetailsViewCourseGroupingModel>> GroupCourses(IList<DraftApprenticeshipDto> draftApprenticeships)
        {
            var apprenticeFundingBandCaps = await Task.WhenAll(draftApprenticeships.Select(async a => new { a.Id, FundingBandCap = await GetFundingBandCap(a.CourseCode, a.StartDate) }));

            var groupedByCourse = draftApprenticeships
                .GroupBy(a => new { a.CourseCode, a.CourseName })
                .Select(course => new DetailsViewCourseGroupingModel
                {
                    CourseCode = course.Key.CourseCode,
                    CourseName = course.Key.CourseName,
                    DraftApprenticeships = course
                        // Sort before on raw properties rather than use displayName property post select for performance reasons
                        .OrderBy(a => a.FirstName)
                        .ThenBy(a => a.LastName)
                        .Join(apprenticeFundingBandCaps, a => a.Id, fc => fc.Id, (a, fc) => new CohortDraftApprenticeshipViewModel
                        {
                            Id = a.Id,
                            DraftApprenticeshipHashedId = _encodingService.Encode(a.Id, EncodingType.ApprenticeshipId),
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Cost = a.Cost,
                            FundingBandCap = fc.FundingBandCap,
                            DateOfBirth = a.DateOfBirth,
                            EndDate = a.EndDate,
                            StartDate = a.StartDate
                        })
                        .Select(a => a)
                .ToList()
            })
            .OrderBy(c => c.CourseName)
                .ToList();

            PopulateFundingBandExcessModels(groupedByCourse);

            return groupedByCourse;
        }

        private static void PopulateFundingBandExcessModels(List<DetailsViewCourseGroupingModel> courseGroups)
        {
            foreach (var courseGroup in courseGroups)
            {
                var apprenticesExceedingFundingBand = courseGroup.DraftApprenticeships.Where(x => x.ExceedsFundingBandCap).ToList();
                int numberExceedingBand = apprenticesExceedingFundingBand.Count;

                if (numberExceedingBand > 0)
                {
                    var fundingExceededValues = apprenticesExceedingFundingBand.GroupBy(x => x.FundingBandCap).Select(fundingBand => fundingBand.Key);
                    courseGroup.FundingBandExcess =
                        new FundingBandExcessModel(apprenticesExceedingFundingBand.Count, fundingExceededValues);
                }
            }
        }

        private async Task<int?> GetFundingBandCap(string courseCode, DateTime? startDate)
        {
            if (startDate == null)
            {
                return null;
            }

            var course = await _trainingProgrammeApiClient.GetTrainingProgramme(courseCode);

            if (course == null)
            {
                return null;
            }

            var cap = course.FundingCapOn(startDate.Value);

            if (cap > 0)
            {
                return cap;
            }

            return null;
        }
    }
}
