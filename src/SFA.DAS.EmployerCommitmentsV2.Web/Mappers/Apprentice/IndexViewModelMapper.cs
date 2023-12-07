using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class IndexViewModelMapper : IMapper<IndexRequest, IndexViewModel>
{
    private readonly ICommitmentsApiClient _client;
    private readonly IModelMapper _modelMapper;
    private readonly IEncodingService _encodingService;

    public IndexViewModelMapper(
        ICommitmentsApiClient client,
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

        var response = await _client.GetApprenticeships(new GetApprenticeshipsRequest
        {
            AccountId = decodedAccountId,
            PageNumber = source.PageNumber,
            PageItemCount = Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage,
            SortField = source.SortField,
            ReverseSort = source.ReverseSort,
            SearchTerm = source.SearchTerm,
            ProviderName = source.SelectedProvider,
            CourseName = source.SelectedCourse,
            Status = source.SelectedStatus,
            EndDate = source.SelectedEndDate,
            Alert = source.SelectedAlert,
            ApprenticeConfirmationStatus = source.SelectedApprenticeConfirmation
        });

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
            var filters = await _client.GetApprenticeshipsFilterValues(
                new GetApprenticeshipFiltersRequest { EmployerAccountId = decodedAccountId });

            filterModel.ProviderFilters = filters.ProviderNames;
            filterModel.CourseFilters = filters.CourseNames;
            filterModel.EndDateFilters = filters.EndDates;
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
            FilterModel = filterModel
        };
    }
}