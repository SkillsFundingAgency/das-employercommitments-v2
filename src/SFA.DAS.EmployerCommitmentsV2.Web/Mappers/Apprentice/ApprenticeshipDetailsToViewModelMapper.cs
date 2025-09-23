using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipDetailsToViewModelMapper : IMapper<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, ApprenticeshipDetailsViewModel>
{
    private readonly IEncodingService _encodingService;

    public ApprenticeshipDetailsToViewModelMapper(IEncodingService encodingService)
    {
        _encodingService = encodingService;
    }

    public Task<ApprenticeshipDetailsViewModel> Map(GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
    {
        var result = new ApprenticeshipDetailsViewModel
        {
            EncodedApprenticeshipId = _encodingService.Encode(source.Id, EncodingType.ApprenticeshipId),
            ApprenticeName = $"{source.FirstName} {source.LastName}",
            ProviderName = source.ProviderName,
            CourseName = source.CourseName,
            PlannedStartDate = source.StartDate,
            PlannedEndDate = source.EndDate,
            Status = source.ApprenticeshipStatus,
            ConfirmationStatus = source.ConfirmationStatus,
            Alerts = source.Alerts.Select(x => x.GetDescription()) ,
            ActualStartDate = source.ActualStartDate
        };

        return Task.FromResult(result);
    }
}