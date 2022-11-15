﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using SFA.DAS.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DetailsViewModelMapper : IMapper<DetailsRequest, DetailsViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly IApprovalsApiClient _approvalsApiClient;

        public DetailsViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService, IApprovalsApiClient approvalsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _approvalsApiClient = approvalsApiClient;
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
            var cohortDetailsTask = _approvalsApiClient.GetCohortDetails(source.AccountId, source.CohortId);
            var draftApprenticeshipsTask = _commitmentsApiClient.GetDraftApprenticeships(source.CohortId);
            var emailOverlapsTask = _commitmentsApiClient.GetEmailOverlapChecks(source.CohortId);

            await Task.WhenAll(cohortTask, draftApprenticeshipsTask, emailOverlapsTask, cohortDetailsTask);

            cohort = await cohortTask;
            var cohortDetails = cohortDetailsTask.Result;
            var draftApprenticeships = (await draftApprenticeshipsTask).DraftApprenticeships;
            var emailOverlaps = (await emailOverlapsTask).ApprenticeshipEmailOverlaps.ToList();

            var courses = await GroupCourses(draftApprenticeships, emailOverlaps, cohort);
            var viewOrApprove = cohort.WithParty == CommitmentsV2.Types.Party.Employer ? "Approve" : "View";
            var isAgreementSigned = await IsAgreementSigned(cohort.AccountLegalEntityId);

            return new DetailsViewModel
            {
                AccountHashedId = source.AccountHashedId,
                CohortReference = source.CohortReference,
                WithParty = cohort.WithParty,
                AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
                LegalEntityName = cohortDetails.LegalEntityName,
                ProviderName = cohortDetails.ProviderName,
                TransferSenderHashedId = cohort.TransferSenderId == null ? null : _encodingService.Encode(cohort.TransferSenderId.Value, EncodingType.PublicAccountId),
                EncodedPledgeApplicationId = cohort.PledgeApplicationId == null ? null : _encodingService.Encode(cohort.PledgeApplicationId.Value, EncodingType.PledgeApplicationId),
                Message = cohort.LatestMessageCreatedByProvider,
                Courses = courses,
                PageTitle = draftApprenticeships.Count == 1
                    ? $"{viewOrApprove} apprentice details"
                    : $"{viewOrApprove} {draftApprenticeships.Count} apprentices' details",
                IsApprovedByProvider = cohort.IsApprovedByProvider,
                IsAgreementSigned = isAgreementSigned,
                IsCompleteForEmployer = cohort.IsCompleteForEmployer,
                HasEmailOverlaps = emailOverlaps.Any(),
                ShowAddAnotherApprenticeOption = !cohort.IsLinkedToChangeOfPartyRequest,
                ShowRofjaaRemovalBanner = cohortDetails.HasUnavailableFlexiJobAgencyDeliveryModel,
                Status = GetCohortStatus(cohort, draftApprenticeships)
            };
        }

        private async Task<IReadOnlyCollection<DetailsViewCourseGroupingModel>> GroupCourses(IEnumerable<DraftApprenticeshipDto> draftApprenticeships, List<ApprenticeshipEmailOverlap> emailOverlaps, GetCohortResponse cohortResponse)
        {
            var groupedByCourse = draftApprenticeships
                .GroupBy(a => new { a.CourseCode, a.CourseName, a.DeliveryModel })
                .Select(course => new DetailsViewCourseGroupingModel
                {
                    CourseCode = course.Key.CourseCode,
                    CourseName = course.Key.CourseName,
                    DeliveryModel = course.Key.DeliveryModel,
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
                            ActualStartDate = a.ActualStartDate,
                            OriginalStartDate = a.OriginalStartDate,
                            HasOverlappingEmail = emailOverlaps.Any(x => x.Id == a.Id),
                            ULN = a.Uln,
                            IsComplete = IsDraftApprenticeshipComplete(a, cohortResponse),
                            EmploymentPrice = a.EmploymentPrice,
                            EmploymentEndDate = a.EmploymentEndDate,
                            IsOnFlexiPaymentPilot = a.IsOnFlexiPaymentPilot
                        })
                .ToList()
                })
            .OrderBy(c => c.CourseName)
                .ToList();

            PopulateFundingBandExcessModels(groupedByCourse);
            await CheckUlnOverlap(groupedByCourse);
            PopulateEmailOverlapsModel(groupedByCourse);

            return groupedByCourse;
        }

        private bool IsDraftApprenticeshipComplete(DraftApprenticeshipDto draftApprenticeship, GetCohortResponse cohortResponse) =>
                !(
                  string.IsNullOrWhiteSpace(draftApprenticeship.FirstName) || string.IsNullOrWhiteSpace(draftApprenticeship.LastName)
                  || draftApprenticeship.DateOfBirth == null || string.IsNullOrWhiteSpace(draftApprenticeship.CourseName)
                  || (draftApprenticeship.StartDate == null && draftApprenticeship.ActualStartDate == null) || draftApprenticeship.EndDate == null || draftApprenticeship.Cost == null
                  || (cohortResponse.ApprenticeEmailIsRequired && string.IsNullOrWhiteSpace(draftApprenticeship.Email) && !cohortResponse.IsLinkedToChangeOfPartyRequest)
                 );

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

                    draftApprenticeship.HasOverlappingUln = result.HasOverlappingStartDate || result.HasOverlappingEndDate;
                }
            }
        }

        private void PopulateEmailOverlapsModel(List<DetailsViewCourseGroupingModel> courseGroups)
        {
            foreach (var courseGroup in courseGroups)
            {
                var numberOfEmailOverlaps = courseGroup.DraftApprenticeships.Count(x => x.HasOverlappingEmail);
                if (numberOfEmailOverlaps > 0)
                {
                    courseGroup.EmailOverlaps = new EmailOverlapsModel(numberOfEmailOverlaps);
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
                    course = await _commitmentsApiClient.GetTrainingProgramme(courseCode);
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

        private string GetCohortStatus(GetCohortResponse cohort, IReadOnlyCollection<DraftApprenticeshipDto> draftApprenticeships)
        {
            if (cohort.TransferSenderId.HasValue &&
                cohort.TransferApprovalStatus == CommitmentsV2.Types.TransferApprovalStatus.Pending)
            {
                if (cohort.WithParty == CommitmentsV2.Types.Party.TransferSender)
                {
                    return "Pending - with funding employer";
                }
                else if (cohort.WithParty == CommitmentsV2.Types.Party.Employer)
                {
                    return GetEmployerOnlyStatus(cohort);
                }
                else if (cohort.WithParty == CommitmentsV2.Types.Party.Provider)
                {
                    return GetProviderOnlyStatus(cohort);
                }
            }
            else if (cohort.IsApprovedByEmployer && cohort.IsApprovedByProvider)
            {
                return "Approved";
            }
            else if (cohort.WithParty == CommitmentsV2.Types.Party.Provider)
            {
                return GetProviderOnlyStatus(cohort);
            }
            else if (cohort.WithParty == CommitmentsV2.Types.Party.Employer)
            {
                return GetEmployerOnlyStatus(cohort);
            }

            return "New request";
        }

        private static string GetEmployerOnlyStatus(GetCohortResponse cohort)
        {
            if (cohort.LastAction == CommitmentsV2.Types.LastAction.None)
            {
                return "New request";
            }
            else if (cohort.LastAction == CommitmentsV2.Types.LastAction.Amend)
            {
                return "Ready for review";
            }
            else if (cohort.LastAction == CommitmentsV2.Types.LastAction.Approve)
            {
                if (!cohort.IsApprovedByProvider && !cohort.IsApprovedByEmployer)
                    return "Ready for review";

                return "Ready for approval";
            }
            else
            {
                return "New request";
            }
        }

        private static string GetProviderOnlyStatus(GetCohortResponse cohort)
        {
            if (cohort.LastAction == CommitmentsV2.Types.LastAction.None)
            {
                return "New request";
            }
            else if (cohort.LastAction == CommitmentsV2.Types.LastAction.Amend)
            {
                return "Under review with provider";
            }
            else if (cohort.LastAction == CommitmentsV2.Types.LastAction.Approve)
            {
                return "With provider for approval";
            }
            else
            {
                return "Under review with provider";
            }
        }
    }
}