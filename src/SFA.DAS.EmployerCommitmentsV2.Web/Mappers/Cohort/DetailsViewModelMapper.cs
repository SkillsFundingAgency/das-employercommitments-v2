using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DetailsViewModelMapper : IMapper<DetailsRequest, DetailsViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IAccountApiClient _accountsApiClient;

        public DetailsViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, 
            IAccountApiClient accountsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<DetailsViewModel> Map(DetailsRequest source)
        {
            GetCohortResponse cohort;

            Task<bool> IsAgreementSigned(long accountLegalEntityId)
            {
                var request = new AgreementSignedRequest
                {
                    AccountLegalEntityId = accountLegalEntityId
                };

                if (cohort.IsFundedByTransfer)
                {
                    request.AgreementFeatures = new AgreementFeature[] { AgreementFeature.Transfers };
                }

                return _commitmentsApiClient.IsAgreementSigned(request);
            }

            var cohortTask = _commitmentsApiClient.GetCohort(source.CohortId);
            var draftApprenticeshipsTask = _commitmentsApiClient.GetDraftApprenticeships(source.CohortId);

            await Task.WhenAll(cohortTask, draftApprenticeshipsTask);

            cohort = await cohortTask;
            var draftApprenticeships = (await draftApprenticeshipsTask).DraftApprenticeships;

            var courses = await GroupCourses(draftApprenticeships);
            var viewOrApprove = cohort.WithParty == CommitmentsV2.Types.Party.Employer ? "Approve" : "View";
            var isAgreementSigned = await IsAgreementSigned(cohort.AccountLegalEntityId);

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
                Courses = courses,
                PageTitle = draftApprenticeships.Count == 1
                    ? $"{viewOrApprove} apprentice details"
                    : $"{viewOrApprove} {draftApprenticeships.Count} apprentices' details",
                IsApprovedByProvider = cohort.IsApprovedByProvider,
                IsAgreementSigned = isAgreementSigned,
                IsCompleteForEmployer = cohort.IsCompleteForEmployer,
                ShowAddAnotherApprenticeOption = !cohort.IsLinkedToChangeOfPartyRequest
            };
        }

        private async Task<IReadOnlyCollection<DetailsViewCourseGroupingModel>> GroupCourses(IEnumerable<DraftApprenticeshipDto> draftApprenticeships)
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
                            StartDate = a.StartDate,
                            OriginalStartDate = a.OriginalStartDate,
                            ULN = a.Uln
                        })
                .ToList()
                })
            .OrderBy(c => c.CourseName)
                .ToList();

            PopulateFundingBandExcessModels(groupedByCourse);
            await CheckUlnOverlap(groupedByCourse);

            return groupedByCourse;
        }

        private Task CheckUlnOverlap(List<DetailsViewCourseGroupingModel> courseGroups)
        {
            var results = courseGroups.Select(courseGroup => SetUlnOverlap(courseGroup.DraftApprenticeships));
            return Task.WhenAll(results);
        }

        private async Task SetUlnOverlap(IReadOnlyCollection<CohortDraftApprenticeshipViewModel> draftApprenticeships)
        {
            foreach (var draftApprenticeship in draftApprenticeships)
            {
                if (!string.IsNullOrWhiteSpace(draftApprenticeship.ULN) && draftApprenticeship.StartDate.HasValue && draftApprenticeship.EndDate.HasValue)
                {
                    var result = await _commitmentsApiClient.ValidateUlnOverlap(new CommitmentsV2.Api.Types.Requests.ValidateUlnOverlapRequest
                    {
                        EndDate = draftApprenticeship.EndDate.Value,
                        StartDate = draftApprenticeship.StartDate.Value,
                        ULN = draftApprenticeship.ULN,
                        ApprenticeshipId = draftApprenticeship.Id
                    });

                    draftApprenticeship.HasOverlappingUln = result.HasOverlappingEndDate || result.HasOverlappingEndDate;
                }
            }
        }

        private void PopulateFundingBandExcessModels(List<DetailsViewCourseGroupingModel> courseGroups)
        {
            var results = courseGroups.Select(courseGroup => SetFundingBandCap(courseGroup.CourseCode, courseGroup.DraftApprenticeships)).ToList();

            Task.WhenAll(results).Wait();
            
            foreach (var courseGroup in courseGroups)
            {
                var apprenticesExceedingFundingBand = courseGroup.DraftApprenticeships.Where(x => x.ExceedsFundingBandCap).ToList();
                int numberExceedingBand = apprenticesExceedingFundingBand.Count;

                if (numberExceedingBand > 0)
                {
                    var fundingExceededValues = apprenticesExceedingFundingBand.GroupBy(x => x.FundingBandCap).Select(fundingBand => fundingBand.Key);
                    var fundingBandCapExcessHeader = GetFundingBandExcessHeader(apprenticesExceedingFundingBand.Count);
                    var fundingBandCapExcessLabel = GetFundingBandExcessLabel(apprenticesExceedingFundingBand.Count);

                    courseGroup.FundingBandExcess =
                        new FundingBandExcessModel(apprenticesExceedingFundingBand.Count, fundingExceededValues, fundingBandCapExcessHeader, fundingBandCapExcessLabel);
                }
            }
        }

        private async Task SetFundingBandCap(string courseCode, IEnumerable<CohortDraftApprenticeshipViewModel> draftApprenticeships)
        {
            GetTrainingProgrammeResponse course = null;
            if (!string.IsNullOrEmpty(courseCode))
            {
                try
                {
                    course =  await _commitmentsApiClient.GetTrainingProgramme(courseCode);
                }
                catch (RestHttpClientException e)
                {
                    if (e.StatusCode != HttpStatusCode.NotFound)
                    {
                        throw;
                    }
                }    
            }
            
            foreach (var draftApprenticeship in draftApprenticeships)
            {
                draftApprenticeship.FundingBandCap = GetFundingBandCap(course, draftApprenticeship.OriginalStartDate ?? draftApprenticeship.StartDate);
            }
        }

        private int? GetFundingBandCap(GetTrainingProgrammeResponse course, DateTime? startDate)
        {
            if (startDate == null)
            {
                return null;
            }

            if (course == null)
            {
                return null;
            }

            var cap = course.TrainingProgramme.FundingCapOn(startDate.Value);

            if (cap > 0)
            {
                return cap;
            }

            return null;
        }

        private string GetFundingBandExcessHeader(int apprenticeshipsOverCap)
        {
            if (apprenticeshipsOverCap == 1)
                return new string($"{apprenticeshipsOverCap} apprenticeship above funding band maximum");
            if (apprenticeshipsOverCap > 1)
                return new string($"{apprenticeshipsOverCap} apprenticeships above funding band maximum");
            return null;
        }

        private string GetFundingBandExcessLabel(int apprenticeshipsOverCap)
        {
            if (apprenticeshipsOverCap == 1)
                return new string("The price for this apprenticeship is above its");
            if (apprenticeshipsOverCap > 1)
                return new string("The price for these apprenticeships is above the");
            return null;
        }
    }
}