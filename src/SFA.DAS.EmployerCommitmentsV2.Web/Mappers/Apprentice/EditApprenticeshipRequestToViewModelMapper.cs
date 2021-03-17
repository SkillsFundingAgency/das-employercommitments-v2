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
            // TODO : convert them into task 
            var apprenticeship = await _commitmentsApiClient.GetApprenticeship(source.ApprenticeshipId, CancellationToken.None);
            var priceEpisodes = await _commitmentsApiClient.GetPriceEpisodes(source.ApprenticeshipId, CancellationToken.None);
            
            var commitment = await _commitmentsApiClient.GetCohort(apprenticeship.CohortId);
            var courseDetails = await _commitmentsApiClient.GetTrainingProgramme(apprenticeship.CourseCode);
            var accountDetails = await _commitmentsApiClient.GetAccount(source.AccountId);
            
            //TODO: Check if it is a transfer sender scenario, should we only show the Standard courses.
            //TODO: check for Change of party scenario, i.e. it changed from levy to non-levy employer and training programme is Framework. 
            var courses = accountDetails.LevyStatus == ApprenticeshipEmployerType.Levy 
                ? (await _commitmentsApiClient.GetAllTrainingProgrammes(CancellationToken.None)).TrainingProgrammes
                : (await _commitmentsApiClient.GetAllTrainingProgrammeStandards(CancellationToken.None)).TrainingProgrammes;

            var isLockedForUpdate = (apprenticeship.Status == ApprenticeshipStatus.Live &&
                                     (apprenticeship.HasHadDataLockSuccess || _currentDateTime.UtcNow > _academicYearDateProvider.LastAcademicYearFundingPeriod &&
                                     !IsWithInFundingPeriod(apprenticeship.StartDate)))
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
            };

            return result;
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
}
