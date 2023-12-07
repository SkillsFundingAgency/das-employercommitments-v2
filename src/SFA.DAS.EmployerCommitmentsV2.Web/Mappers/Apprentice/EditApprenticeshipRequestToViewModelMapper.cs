using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditApprenticeshipRequestToViewModelMapper : IMapper<EditApprenticeshipRequest, EditApprenticeshipRequestViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IAcademicYearDateProvider _academicYearDateProvider;
    private readonly ICurrentDateTime _currentDateTime;
    private readonly IEncodingService _encodingService;
    private readonly IApprovalsApiClient _apiClient;

    public EditApprenticeshipRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IAcademicYearDateProvider academicYearDateProvider, ICurrentDateTime currentDateTime, IEncodingService encodingService, IApprovalsApiClient apiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _academicYearDateProvider = academicYearDateProvider;
        _currentDateTime = currentDateTime;
        _encodingService = encodingService;
        _apiClient = apiClient;
    }
    public async Task<EditApprenticeshipRequestViewModel> Map(EditApprenticeshipRequest source)
    {
        var editApprenticeshipTask =  _apiClient.GetEditApprenticeship(source.AccountId, source.ApprenticeshipId);
        var apprenticeshipTask = _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);
        var priceEpisodesTask = _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId, CancellationToken.None);
        var accountDetailsTask = _commitmentsApiClient.GetAccount(source.AccountId);

        await Task.WhenAll(editApprenticeshipTask, apprenticeshipTask, accountDetailsTask, priceEpisodesTask);

        var apprenticeship = apprenticeshipTask.Result;
        var editApprenticeship = editApprenticeshipTask.Result;
        var accountDetails = accountDetailsTask.Result;
        var priceEpisodes = priceEpisodesTask.Result;

        var courses = accountDetails.LevyStatus == ApprenticeshipEmployerType.NonLevy || editApprenticeship.IsFundedByTransfer
            ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards(CancellationToken.None)).TrainingProgrammes
            : (await _commitmentsApiClient.GetAllTrainingProgrammes(CancellationToken.None)).TrainingProgrammes;

        var isLockedForUpdate = IsLiveAndHasHadDataLockSuccess(apprenticeship)
                                ||
                                IsLiveAndIsNotWithInFundingPeriod(apprenticeship)
                                ||
                                IsPausedAndHasHadDataLockSuccess(apprenticeship)
                                ||
                                IsPausedAndIsNotWithInFundingPeriod(apprenticeship)
                                ||
                                IsPausedAndHasHadDataLockSuccessAndIsFundedByTransfer(apprenticeship, editApprenticeship.IsFundedByTransfer)
                                ||
                                IsWaitingToStartAndHasHadDataLockSuccessAndIsFundedByTransfer(apprenticeship, editApprenticeship.IsFundedByTransfer);

        var result = new EditApprenticeshipRequestViewModel(apprenticeship.DateOfBirth, apprenticeship.StartDate, apprenticeship.EndDate, apprenticeship.EmploymentEndDate)
        {
            FirstName = apprenticeship.FirstName,
            LastName = apprenticeship.LastName,
            Email = apprenticeship.Email,
            ULN= apprenticeship.Uln,
            DeliveryModel = apprenticeship.DeliveryModel,
            CourseCode = apprenticeship.CourseCode,
            Version = apprenticeship.Version,
            Option = apprenticeship.Option == string.Empty ? "TBC" : apprenticeship.Option,
            Cost = priceEpisodes.PriceEpisodes.GetPrice(),
            EmployerReference = apprenticeship.EmployerReference,
            Courses = courses,
            IsContinuation = apprenticeship.IsContinuation,
            IsLockedForUpdate = isLockedForUpdate,
            IsUpdateLockedForStartDateAndCourse = editApprenticeship.IsFundedByTransfer && !apprenticeship.HasHadDataLockSuccess,
            IsEndDateLockedForUpdate = IsEndDateLocked(isLockedForUpdate, apprenticeship.HasHadDataLockSuccess, apprenticeship.Status),
            TrainingName = apprenticeship.CourseName,
            HashedApprenticeshipId = source.ApprenticeshipHashedId,
            AccountHashedId = source.AccountHashedId,
            EmailAddressConfirmedByApprentice = apprenticeship.EmailAddressConfirmedByApprentice,
            EmailShouldBePresent = apprenticeship.EmailShouldBePresent,
            EmploymentPrice = apprenticeship.EmploymentPrice,
            ProviderId = apprenticeship.ProviderId,
            AccountLegalEntityHashedId = _encodingService.Encode(apprenticeship.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
            HasMultipleDeliveryModelOptions = editApprenticeship.HasMultipleDeliveryModelOptions
        };

        return result;
    }

    private bool IsPausedAndHasHadDataLockSuccessAndIsFundedByTransfer(GetApprenticeshipResponse apprenticeship, bool isFundedByTransfer)
    {
        if (CheckWaitingToStart(apprenticeship)) return isFundedByTransfer && HasHadDataLockSuccess(apprenticeship) && IsPaused(apprenticeship); else return false;
    }

    private bool IsPausedAndHasHadDataLockSuccess(GetApprenticeshipResponse apprenticeship)
    {
        if (!CheckWaitingToStart(apprenticeship)) return IsPaused(apprenticeship) && HasHadDataLockSuccess(apprenticeship); else return false;
    }

    private bool IsPausedAndIsNotWithInFundingPeriod(GetApprenticeshipResponse apprenticeship)
    {
        if (!CheckWaitingToStart(apprenticeship)) return IsPaused(apprenticeship) && !IsWithInFundingPeriod(apprenticeship.StartDate.Value); else return false;
    }

    private bool IsPaused(GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.Status == ApprenticeshipStatus.Paused;
    }

    private bool CheckWaitingToStart(GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.StartDate.Value > new DateTime(_currentDateTime.UtcNow.Year, _currentDateTime.UtcNow.Month, 1);
    }

    private bool IsWaitingToStartAndHasHadDataLockSuccessAndIsFundedByTransfer(GetApprenticeshipResponse apprenticeship, bool isFundedByTransfer)
    {
        return isFundedByTransfer
               && HasHadDataLockSuccess(apprenticeship) 
               && IsWaitingToStart(apprenticeship);
    }

    private bool IsLiveAndHasHadDataLockSuccess(GetApprenticeshipResponse apprenticeship)
    {
        return IsLive(apprenticeship) && HasHadDataLockSuccess(apprenticeship);
    }

    private bool IsLiveAndIsNotWithInFundingPeriod(GetApprenticeshipResponse apprenticeship)
    {
        return IsLive(apprenticeship) && !IsWithInFundingPeriod(apprenticeship.StartDate.Value);
    }

    private bool IsWaitingToStart(GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.Status == ApprenticeshipStatus.WaitingToStart;
    }

    private bool IsLive(GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.Status == ApprenticeshipStatus.Live;
    }

    private bool HasHadDataLockSuccess(GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.HasHadDataLockSuccess;
    }

    private bool IsEndDateLocked(bool isLockedForUpdate, bool hasHadDataLockSuccess, ApprenticeshipStatus status)
    {
        var result = isLockedForUpdate;
        if (hasHadDataLockSuccess)
        {
            result = status == ApprenticeshipStatus.WaitingToStart;
        }

        return result;
    }

    private bool IsWithInFundingPeriod(DateTime trainingStartDate)
    {
        if (trainingStartDate < _academicYearDateProvider.CurrentAcademicYearStartDate &&
            _currentDateTime.UtcNow > _academicYearDateProvider.LastAcademicYearFundingPeriod)
        {
            return false;
        }

        return true;
    }
}