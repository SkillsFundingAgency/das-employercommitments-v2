using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice
{
    public class DataLockRequestRestartRequestToViewModelMapper : IMapper<DataLockRequestRestartRequest, DataLockRequestRestartViewModel>
    {
        private readonly ICommitmentsApiClient _commitmentsApiClient;

        public DataLockRequestRestartRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<DataLockRequestRestartViewModel> Map(DataLockRequestRestartRequest source)
        {
            var dataLockSummariesTask = _commitmentsApiClient
                .GetApprenticeshipDatalockSummariesStatus(source.ApprenticeshipId);

            var apprenticeshipTask = _commitmentsApiClient
                .GetApprenticeship(source.ApprenticeshipId);

            var trainingProgrammesTask = _commitmentsApiClient.GetAllTrainingProgrammes();

            await Task.WhenAll(dataLockSummariesTask, apprenticeshipTask, trainingProgrammesTask);

            var dataLockSummaries = dataLockSummariesTask.Result;
            
            var dataLock = dataLockSummaries.DataLocksWithCourseMismatch
                .FirstOrDefault(m => m.TriageStatus == TriageStatus.Restart);

            if (dataLock == null)
                throw new Exception($"No data locks exist that can be restarted for apprenticeship: {source.ApprenticeshipId}");

            var apprenticeship = apprenticeshipTask.Result;
            
            var trainingProgrammes = trainingProgrammesTask.Result;
            var newProgramme = trainingProgrammes.TrainingProgrammes.Single(m => m.CourseCode == dataLock.IlrTrainingCourseCode);

            return new DataLockRequestRestartViewModel
            {
                ApprenticeshipHashedId = source.ApprenticeshipHashedId,
                AccountHashedId = source.AccountHashedId,
                AccountId = source.AccountId,
                ApprenticeshipId = source.ApprenticeshipId,
                ProviderName = apprenticeship.ProviderName,
                OriginalApprenticeship = GetOriginalApprenticeship(apprenticeship),
                NewCourseCode = newProgramme.CourseCode,
                NewCourseName = newProgramme.Name,
                NewPeriodStartDate = dataLock.IlrEffectiveFromDate,
                NewPeriodEndDate = dataLock.IlrPriceEffectiveToDate,
            };
        }

        private BaseEdit GetOriginalApprenticeship(GetApprenticeshipResponse apprenticeship)
        {
            var OriginalApprenticeship = new BaseEdit
            {
                FirstName = apprenticeship.FirstName,
                LastName = apprenticeship.LastName,
                DateOfBirth = apprenticeship.DateOfBirth,
                ULN = apprenticeship.Uln,
                StartDate = apprenticeship.StartDate,
                EndDate = apprenticeship.EndDate,
                CourseCode = apprenticeship.CourseCode,
                CourseName = apprenticeship.CourseName
            };

            return OriginalApprenticeship;
        }
    }
}
