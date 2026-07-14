using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
{
    private readonly IApprovalsApiClient _client;
    private readonly IModelMapper _modelMapper;
    private readonly IEncodingService _encodingService;

    public IndexViewModelMapper(
        IApprovalsApiClient client,
        IModelMapper modelMapper,
        IEncodingService encodingService)
    {
        _client = client;
        _modelMapper = modelMapper;
        _encodingService = encodingService;
    }

    public async Task<IndexViewModel> Map(IndexRequest source)
    {
        var decodedAccountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);

        var response = await _client.GetApprenticeships(new
            GetApprenticeshipsRequest(
            decodedAccountId,
            source.PageNumber,
            Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage,
            source.SortField,
            source.ReverseSort,
            source.SearchTerm,
            null, source.SelectedProvider,
            source.SelectedCourse,
            source.SelectedStatus,
            null,
            source.SelectedEndDate, null, null, null,
            source.SelectedAlert,
            source.SelectedApprenticeConfirmation,
            null));

        var statusFilters = new[]
        {
            ApprenticeshipStatus.WaitingToStart,
            ApprenticeshipStatus.Live,
            ApprenticeshipStatus.Paused,
            ApprenticeshipStatus.Stopped,
            ApprenticeshipStatus.Completed
        };

        var alertFilters = new[]
        {
            Alerts.ChangesForReview ,
            Alerts.ChangesPending,
            Alerts.ChangesRequested,
            Alerts.ConfirmDates
        };

        var filterModel = new ApprenticesFilterModel
        {
            AccountHashedId = source.AccountHashedId,
            TotalNumberOfApprenticeships = response.TotalApprenticeships,
            TotalNumberOfApprenticeshipsFound = response.TotalApprenticeshipsFound,
            TotalNumberOfApprenticeshipsWithAlertsFound = response.TotalApprenticeshipsWithAlertsFound,
            PageNumber = response.PageNumber,
            SortField = source.SortField,
            ReverseSort = source.ReverseSort,
            SearchTerm = source.SearchTerm,
            SelectedProvider = source.SelectedProvider,
            SelectedCourse = source.SelectedCourse,
            SelectedStatus = source.SelectedStatus,
            SelectedAlert = source.SelectedAlert,
            SelectedApprenticeConfirmation = source.SelectedApprenticeConfirmation,
            SelectedEndDate = source.SelectedEndDate,
            StatusFilters = statusFilters,
            AlertFilters = alertFilters
        };

        if (response.TotalApprenticeships >= Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch)
        {
            filterModel.ProviderFilters = response.ApprenticeshipFiltersValue.ProviderNames;
            filterModel.CourseFilters = response.ApprenticeshipFiltersValue.CourseNames;
            filterModel.EndDateFilters = response.ApprenticeshipFiltersValue.EndDates;
        }

        var apprenticeships = new List<ApprenticeshipDetailsViewModel>();

        foreach (var apprenticeshipDetailsResponse in response.Apprenticeships)
        {
            var apprenticeship = await _modelMapper.Map<ApprenticeshipDetailsViewModel>(apprenticeshipDetailsResponse);
            apprenticeships.Add(apprenticeship);
        }

        return new IndexViewModel
        {
            AccountHashedId = source.AccountHashedId,
            Apprenticeships = apprenticeships,
            FilterModel = filterModel,
            HasChangeHistory = response.HasChangeHistory
        };
    }
}