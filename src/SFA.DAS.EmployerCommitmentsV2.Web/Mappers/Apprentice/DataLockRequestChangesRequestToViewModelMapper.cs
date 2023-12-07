using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class DataLockRequestChangesRequestToViewModelMapper : IMapper<DataLockRequestChangesRequest, DataLockRequestChangesViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public DataLockRequestChangesRequestToViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<DataLockRequestChangesViewModel> Map(DataLockRequestChangesRequest source)
    {
        var dataLockSummariesTask = _commitmentsApiClient
            .GetApprenticeshipDatalockSummariesStatus(source.ApprenticeshipId);

        var priceEpisodesTask = _commitmentsApiClient
            .GetPriceEpisodes(source.ApprenticeshipId);

        var apprenticeshipTask = _commitmentsApiClient
            .GetApprenticeship(source.ApprenticeshipId);

        var trainingProgrammesTask = _commitmentsApiClient.GetAllTrainingProgrammes();

        await Task.WhenAll(dataLockSummariesTask, priceEpisodesTask, apprenticeshipTask, trainingProgrammesTask);

        var dataLockSummaries = dataLockSummariesTask.Result;
        var priceEpisodes = priceEpisodesTask.Result;
        var apprenticeship = apprenticeshipTask.Result;
        var trainingProgrammes = trainingProgrammesTask.Result;

        var dataLocksPrice =
            dataLockSummaries.DataLocksWithCourseMismatch
                .Concat(dataLockSummaries.DataLocksWithOnlyPriceMismatch)
                .Where(m => m.ErrorCode.HasFlag(DataLockErrorCode.Dlock07));

        return new DataLockRequestChangesViewModel
        {
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            AccountHashedId = source.AccountHashedId,
            AccountId = source.AccountId,
            ApprenticeshipId = source.ApprenticeshipId,
            ProviderName = apprenticeship.ProviderName,
            OriginalApprenticeship = GetOriginalApprenticeship(apprenticeship),
            CourseChanges = MapCourseChanges(dataLockSummaries.DataLocksWithCourseMismatch, apprenticeship, priceEpisodes.PriceEpisodes, trainingProgrammes.TrainingProgrammes),
            PriceChanges = MapPriceChanges(dataLocksPrice, priceEpisodes.PriceEpisodes)
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

    private IList<DataLockPriceChange> MapPriceChanges(IEnumerable<DataLock> dataLocks, IReadOnlyCollection<GetPriceEpisodesResponse.PriceEpisode> priceEpisodes)
    {
        return dataLocks
            .Select(dataLock =>
            {
                var previousPriceEpisode = priceEpisodes
                    .OrderByDescending(m => m.FromDate)
                    .FirstOrDefault(m => m.FromDate <= dataLock.IlrEffectiveFromDate);

                if (previousPriceEpisode == null)
                {
                    previousPriceEpisode = priceEpisodes
                        .OrderByDescending(m => m.FromDate)
                        .FirstOrDefault();
                }

                return new DataLockPriceChange
                {
                    CurrentStartDate = previousPriceEpisode?.FromDate ?? DateTime.MinValue,
                    CurrentEndDate = previousPriceEpisode?.ToDate,
                    CurrentCost = previousPriceEpisode?.Cost ?? default(decimal),
                    IlrStartDate = dataLock.IlrEffectiveFromDate ?? DateTime.MinValue,
                    IlrEndDate = dataLock.IlrPriceEffectiveToDate,
                    IlrCost = dataLock.IlrTotalCost ?? default(decimal),
                    MissingPriceHistory = previousPriceEpisode == null
                };
            }).ToList();
    }

    private IList<DataLockCourseChange> MapCourseChanges(IEnumerable<DataLock> dataLocks, GetApprenticeshipResponse apprenticeship, IReadOnlyCollection<GetPriceEpisodesResponse.PriceEpisode> priceHistory, IEnumerable<TrainingProgramme> trainingProgrammes)
    {
        var earliestPriceHistory = priceHistory.Min(x => x.FromDate);

        return dataLocks
            .Where(m => m.TriageStatus == TriageStatus.Change)
            .Select(dataLock =>
            {
                return new DataLockCourseChange
                {
                    CurrentStartDate = earliestPriceHistory,
                    CurrentEndDate = apprenticeship.EndDate,
                    CurrentTrainingProgram = apprenticeship.CourseName,
                    IlrStartDate = dataLock.IlrEffectiveFromDate.Value,
                    IlrEndDate = dataLock.IlrPriceEffectiveToDate,
                    IlrTrainingProgram = trainingProgrammes.FirstOrDefault(p => p.CourseCode == dataLock.IlrTrainingCourseCode)?.Name
                };
            }).ToList();
    }
}