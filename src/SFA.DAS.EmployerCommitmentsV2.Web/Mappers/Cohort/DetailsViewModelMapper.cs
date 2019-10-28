using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
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
        private readonly IEmployerAgreementService _employerAgreementService;

        public DetailsViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, 
            ITrainingProgrammeApiClient trainingProgrammeApiClient, IEmployerAgreementService employerAgreementService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _trainingProgrammeApiClient = trainingProgrammeApiClient;
            _employerAgreementService = employerAgreementService;
        }

        public async Task<DetailsViewModel> Map(DetailsRequest source)
        {
            GetCohortResponse cohort;

            async Task<bool> IsAgreementSigned()
            {
                var ale = await _commitmentsApiClient.GetLegalEntity(cohort.AccountLegalEntityId);
                var maLegalEntityId = ale.MaLegalEntityId;

                if (cohort.IsFundedByTransfer)
                {
                    return await _employerAgreementService.IsAgreementSigned(source.AccountId, maLegalEntityId,
                        AgreementFeature.Transfers);
                }
                return await _employerAgreementService.IsAgreementSigned(source.AccountId, maLegalEntityId);
            }

            var cohortTask = _commitmentsApiClient.GetCohort(source.CohortId);
            var draftApprenticeshipsTask = _commitmentsApiClient.GetDraftApprenticeships(source.CohortId);

            await Task.WhenAll(cohortTask, draftApprenticeshipsTask);

            cohort = await cohortTask;
            var draftApprenticeships = (await draftApprenticeshipsTask).DraftApprenticeships;

            var viewOrApprove = cohort.WithParty == CommitmentsV2.Types.Party.Employer ? "Approve" : "View";

            return new DetailsViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                WithParty = cohort.WithParty,
                AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                TransferSenderHashedId = cohort.TransferSenderId == null ? null : _encodingService.Encode(cohort.TransferSenderId.Value, EncodingType.PublicAccountId),
                Message = cohort.LatestMessageCreatedByProvider,
                Courses = GroupCourses(draftApprenticeships),
                PageTitle = draftApprenticeships.Count == 1
                    ? $"{viewOrApprove} apprentice details"
                    : $"{viewOrApprove} {draftApprenticeships.Count} apprentices' details",
                IsApprovedByProvider = cohort.IsApprovedByProvider,
                IsAgreementSigned = await IsAgreementSigned()
            };
        }

        private IReadOnlyCollection<DetailsViewCourseGroupingModel> GroupCourses(IEnumerable<DraftApprenticeshipDto> draftApprenticeships)
        {
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
                        .Select(a => new CohortDraftApprenticeshipViewModel
                        {
                            Id = a.Id,
                            DraftApprenticeshipHashedId = _encodingService.Encode(a.Id, EncodingType.ApprenticeshipId),
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Cost = a.Cost,
                            DateOfBirth = a.DateOfBirth,
                            EndDate = a.EndDate,
                            StartDate = a.StartDate
                        })
                .ToList()
                })
            .OrderBy(c => c.CourseName)
                .ToList();

            PopulateFundingBandExcessModels(groupedByCourse);

            return groupedByCourse;
        }

        private void PopulateFundingBandExcessModels(List<DetailsViewCourseGroupingModel> courseGroups)
        {
            Parallel.ForEach(courseGroups, group => SetFundingBandCap(group.CourseCode, group.DraftApprenticeships));
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

        private void SetFundingBandCap(string courseCode, IEnumerable<CohortDraftApprenticeshipViewModel> draftApprenticeships)
        {
            Parallel.ForEach(draftApprenticeships, async a => a.FundingBandCap = await GetFundingBandCap(courseCode, a.StartDate));
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