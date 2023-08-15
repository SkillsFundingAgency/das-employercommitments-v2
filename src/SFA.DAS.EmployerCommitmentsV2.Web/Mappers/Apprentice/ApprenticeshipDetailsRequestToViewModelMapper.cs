using Microsoft.Extensions.Logging;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse.GetApprenticeshipUpdateResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class ApprenticeshipDetailsRequestToViewModelMapper : IMapper<ApprenticeshipDetailsRequest, ApprenticeshipDetailsRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IEncodingService _encodingService;
        private readonly ILogger<ApprenticeshipDetailsRequestToViewModelMapper> _logger;
        private readonly IApprovalsApiClient _approvalsApiClient;

        public ApprenticeshipDetailsRequestToViewModelMapper(
            ICommitmentsApiClient commitmentsApiClient,
            IEncodingService encodingService,
            IApprovalsApiClient approvalsApiClient,
            ILogger<ApprenticeshipDetailsRequestToViewModelMapper> logger)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
            _approvalsApiClient = approvalsApiClient;
            _logger = logger;
        }

        public async Task<ApprenticeshipDetailsRequestViewModel> Map(ApprenticeshipDetailsRequest source)
        {
            try
            {
                var apprenticeshipId = _encodingService.Decode(source.ApprenticeshipHashedId, EncodingType.ApprenticeshipId);
                var accountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);

                var response = await _approvalsApiClient.GetManageApprenticeshipDetails(accountId, apprenticeshipId, cancellationToken: CancellationToken.None);

                var currentTrainingProgramme = await GetTrainingProgramme(response.Apprenticeship.CourseCode, response.Apprenticeship.StandardUId);

                PendingChanges pendingChange = GetPendingChanges(response.ApprenticeshipUpdates);

                bool dataLockCourseTriaged = response.DataLocks.HasDataLockCourseTriaged();
                bool dataLockCourseChangedTraiged = response.DataLocks.HasDataLockCourseChangeTriaged();
                bool dataLockPriceTriaged = response.DataLocks.HasDataLockPriceTriaged();

                var pendingChangeOfProviderRequest = response.ChangeOfPartyRequests?
                    .Where(x => x.ChangeOfPartyType == ChangeOfPartyRequestType.ChangeProvider && x.Status == ChangeOfPartyRequestStatus.Pending).FirstOrDefault();

                var hasPendingoverlappingTrainingDateRequest = response.OverlappingTrainingDateRequest != null &&
                    response?.OverlappingTrainingDateRequest?.Any(x => x.Status == OverlappingTrainingDateRequestStatus.Pending) == true;

                bool enableEdit = EnableEdit(response.Apprenticeship, pendingChange, dataLockCourseTriaged, dataLockCourseChangedTraiged, dataLockPriceTriaged, hasPendingoverlappingTrainingDateRequest);

                var apprenticeshipDetails = await _approvalsApiClient.GetApprenticeshipDetails(response.Apprenticeship.ProviderId, apprenticeshipId, CancellationToken.None);

                var result = new ApprenticeshipDetailsRequestViewModel
                {
                    HashedApprenticeshipId = source.ApprenticeshipHashedId,
                    AccountHashedId = source.AccountHashedId,
                    ApprenticeName = $"{response.Apprenticeship.FirstName} {response.Apprenticeship.LastName}",
                    ULN = response.Apprenticeship.Uln,
                    DateOfBirth = response.Apprenticeship.DateOfBirth,
                    StartDate = response.Apprenticeship.StartDate,
                    ActualStartDate = response.Apprenticeship.ActualStartDate,
                    EndDate = response.Apprenticeship.EndDate,
                    StopDate = response.Apprenticeship.StopDate,
                    PauseDate = response.Apprenticeship.PauseDate,
                    CompletionDate = response.Apprenticeship.CompletionDate,
                    TrainingName = currentTrainingProgramme.Name,
                    DeliveryModel = response.Apprenticeship.DeliveryModel,
                    Version = response.Apprenticeship.Version,
                    TrainingType = currentTrainingProgramme.ProgrammeType,
                    Cost = response.PriceEpisodes.GetPrice(),
                    ApprenticeshipStatus = response.Apprenticeship.Status,
                    ProviderName = response.Apprenticeship.ProviderName,
                    PendingChanges = pendingChange,
                    EmployerReference = response.Apprenticeship.EmployerReference,
                    CohortReference = _encodingService.Encode(response.Apprenticeship.CohortId, EncodingType.CohortReference),
                    EnableEdit = enableEdit,
                    EndpointAssessorName = response.Apprenticeship.EndpointAssessorName,
                    MadeRedundant = response.Apprenticeship.MadeRedundant,
                    HasPendingChangeOfProviderRequest = pendingChangeOfProviderRequest != null,
                    PendingChangeOfProviderRequestWithParty = pendingChangeOfProviderRequest?.WithParty,
                    HasContinuation = response.Apprenticeship.HasContinuation,
                    TrainingProviderHistory = response.ChangeOfProviderChain
                        .Select(copc => new TrainingProviderHistory
                        {
                            ProviderName = copc.ProviderName,
                            FromDate = copc.StartDate.Value,
                            ToDate = copc.StopDate.HasValue ? copc.StopDate.Value : copc.EndDate.Value,
                            HashedApprenticeshipId = _encodingService.Encode(copc.ApprenticeshipId, EncodingType.ApprenticeshipId),
                            ShowLink = response.Apprenticeship.Id != copc.ApprenticeshipId
                        })
                        .ToList(),

                    PendingDataLockChange = dataLockPriceTriaged || dataLockCourseChangedTraiged,
                    PendingDataLockRestart = dataLockCourseTriaged,
                    ConfirmationStatus = response.Apprenticeship.ConfirmationStatus,
                    Email = response.Apprenticeship.Email,
                    EmailShouldBePresent = response.Apprenticeship.EmailShouldBePresent,
                    HasNewerVersions = await HasNewerVersions(currentTrainingProgramme),
                    Option = response.Apprenticeship.Option,
                    VersionOptions = currentTrainingProgramme.Options,
                    EmailAddressConfirmedByApprentice = response.Apprenticeship.EmailAddressConfirmedByApprentice,
                    EmploymentEndDate = response.Apprenticeship.EmploymentEndDate,
                    EmploymentPrice = response.Apprenticeship.EmploymentPrice,
                    RecognisePriorLearning = response.Apprenticeship.RecognisePriorLearning,
                    DurationReducedBy = response.Apprenticeship.DurationReducedBy,
                    PriceReducedBy = response.Apprenticeship.PriceReducedBy,
                    HasPendingOverlappingTrainingDateRequest = hasPendingoverlappingTrainingDateRequest,
                    HasMultipleDeliveryModelOptions = response.HasMultipleDeliveryModelOptions,
                    IsOnFlexiPaymentPilot = response.Apprenticeship.IsOnFlexiPaymentPilot
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error mapping for accountId {source.AccountHashedId}  and apprenticeship {source.ApprenticeshipHashedId} to ApprenticeshipDetailsRequestViewModel");
                throw;
            }
        }

        private async Task<TrainingProgramme> GetTrainingProgramme(string courseCode, string standardUId)
        {
            if (!string.IsNullOrEmpty(standardUId))
            {
                var trainingProgrammeVersionResponse = await _commitmentsApiClient.GetTrainingProgrammeVersionByStandardUId(standardUId);

                return trainingProgrammeVersionResponse.TrainingProgramme;
            }
            else
            {
                var frameworkResponse = await _commitmentsApiClient.GetTrainingProgramme(courseCode);

                return frameworkResponse.TrainingProgramme;
            }
        }

        private static bool EnableEdit(GetManageApprenticeshipDetailsResponse.GetApprenticeshipResponse apprenticeship, PendingChanges pendingChange,
            bool dataLockCourseTriaged, bool dataLockCourseChangedTraiged, bool dataLockPriceTriaged, bool hasPendingoverlappingTrainingDateRequest)
        {
            return pendingChange == PendingChanges.None
                            && !dataLockCourseTriaged
                            && !dataLockCourseChangedTraiged
                            && !dataLockPriceTriaged
                            && new[] { ApprenticeshipStatus.WaitingToStart, ApprenticeshipStatus.Live, ApprenticeshipStatus.Paused }.Contains(apprenticeship.Status)
                            && !hasPendingoverlappingTrainingDateRequest;
        }

        private static PendingChanges GetPendingChanges(IEnumerable<ApprenticeshipUpdate> apprenticeshipUpdates)
        {
            var pendingChange = PendingChanges.None;
            if (apprenticeshipUpdates.Any(x => x.OriginatingParty == Party.Employer))
                pendingChange = PendingChanges.WaitingForApproval;
            if (apprenticeshipUpdates.Any(x => x.OriginatingParty == Party.Provider))
                pendingChange = PendingChanges.ReadyForApproval;
            return pendingChange;
        }

        private async Task<bool> HasNewerVersions(TrainingProgramme trainingProgramme)
        {
            if (trainingProgramme.ProgrammeType == ProgrammeType.Standard)
            {
                var newerVersionsResponse = await _commitmentsApiClient.GetNewerTrainingProgrammeVersions(trainingProgramme.StandardUId);

                if (newerVersionsResponse?.NewerVersions != null && newerVersionsResponse.NewerVersions.Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}