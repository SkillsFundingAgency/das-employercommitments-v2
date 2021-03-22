using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class EditApprenticeshipRequestToViewModelMapper : IMapper<EditApprenticeshipRequest, EditApprenticeshipRequestViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly IAcademicYearDateProvider _academicYearDateProvider;
        private readonly ICurrentDateTime _currentDateTime;

        public EditApprenticeshipRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IAcademicYearDateProvider academicYearDateProvider, ICurrentDateTime currentDateTime)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _academicYearDateProvider = academicYearDateProvider;
            _currentDateTime = currentDateTime;
        }
        public async Task<EditApprenticeshipRequestViewModel> Map(EditApprenticeshipRequest source)
        {
            var apprenticeshipTask = _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);
            var priceEpisodesTask = _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId, CancellationToken.None);
            var accountDetailsTask = _commitmentsApiClient.GetAccount(source.AccountId);

            var apprenticeship = await apprenticeshipTask;
            var commitmentTask = _commitmentsApiClient.GetCohort(apprenticeship.CohortId);
            var courseDetailsTask = _commitmentsApiClient.GetTrainingProgramme(apprenticeship.CourseCode);

            var accountDetails = await accountDetailsTask;
            var priceEpisodes = await priceEpisodesTask;
            var commitment = await commitmentTask;
            var courseDetails = await courseDetailsTask;

            var courses = accountDetails.LevyStatus == ApprenticeshipEmployerType.NonLevy || commitment.IsFundedByTransfer
                ? (await _commitmentsApiClient.GetAllTrainingProgrammeStandards(CancellationToken.None)).TrainingProgrammes
                : (await _commitmentsApiClient.GetAllTrainingProgrammes(CancellationToken.None)).TrainingProgrammes;

            var isLockedForUpdate = (apprenticeship.Status == ApprenticeshipStatus.Live &&
                                     (apprenticeship.HasHadDataLockSuccess || !IsWithInFundingPeriod(apprenticeship.StartDate)))
                                    ||
                                    (commitment.IsFundedByTransfer
                                     && apprenticeship.HasHadDataLockSuccess && apprenticeship.Status == ApprenticeshipStatus.WaitingToStart);

            var result = new EditApprenticeshipRequestViewModel(apprenticeship.DateOfBirth, apprenticeship.StartDate, apprenticeship.EndDate)
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                ULN= apprenticeship.Uln,
                CourseCode = apprenticeship.CourseCode,
                Cost = priceEpisodes.PriceEpisodes.GetPrice(),
                Reference = apprenticeship.EmployerReference,
                Courses = courses,
                IsContinuation = apprenticeship.IsContinuation,
                IsLockedForUpdate = isLockedForUpdate,
                IsUpdateLockedForStartDateAndCourse = commitment.IsFundedByTransfer && !apprenticeship.HasHadDataLockSuccess,
                IsEndDateLockedForUpdate = IsEndDateLocked(isLockedForUpdate, apprenticeship.HasHadDataLockSuccess, apprenticeship.Status),
                TrainingName = courseDetails.TrainingProgramme.Name,
                HashedApprenticeshipId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId
            };

            return result;
        }

        private bool IsEndDateLocked(bool isLockedForUpdate, bool hasHadDataLockSuccess, ApprenticeshipStatus status)
        {
            // thoughts on this - the only reason for if condition I can think of is because
            // NotTransferSender :  IsLockedForUpdate becomes true only if it is live. In this case we want to lock the field even if it is waiting for start and we hadAdatalockSuccess
            // TransferSender    : IsLockedForUpdate become true, in case it is Transfer sender and it has received a datalock success even in the case of waiting for start.
            // this condition is valid when it is a not a transfer sender scenario & it is waiting to start, in that case we will lock the end date as well.
            // But this will also cause this field to become editable, if has received a data lock and status is not waiting to start
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
}
