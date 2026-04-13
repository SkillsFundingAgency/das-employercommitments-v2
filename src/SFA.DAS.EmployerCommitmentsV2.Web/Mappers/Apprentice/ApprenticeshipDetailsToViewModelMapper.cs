using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ApprenticeshipDetailsToViewModelMapper(IEncodingService encodingService)
    : IMapper<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, ApprenticeshipDetailsViewModel>
{
    public Task<ApprenticeshipDetailsViewModel> Map(GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
    {
        var result = new ApprenticeshipDetailsViewModel
        {
            EncodedApprenticeshipId = encodingService.Encode(source.Id, EncodingType.ApprenticeshipId),
            ApprenticeName = $"{source.FirstName} {source.LastName}",
            ProviderName = source.ProviderName,
            CourseName = source.CourseName,
            PlannedStartDate = source.StartDate,
            PlannedEndDate = source.EndDate,
            Status = source.ApprenticeshipStatus,
            ConfirmationStatus = source.ConfirmationStatus,
            EmploymentStatus = MapEmploymentStatus(source.EmployerVerificationStatus, source.EmployerVerificationNotes),
            Alerts = source.Alerts.Select(x => x.GetDescription()) ,
            ActualStartDate = source.ActualStartDate
        };

        return Task.FromResult(result);
    }

    private static string MapEmploymentStatus(int? status, string notes)
    {
        return status switch
        {
            null => string.Empty,
            0 => string.Empty,
            2 => "Employed",
            3 => "Not Employed",
            _ => notes switch
            {
                "NinoAndPAYENotFound" => "Not verified - missing PAYE scheme and invalid NINO",
                "NinoFailure" => "Not Verified - missing or invalid NINO",
                "NinoInvalid" => "Not Verified - missing or invalid NINO",
                "NinoNotFound" => "Not verified - invalid NINO",
                "PAYENotFound" => "Not verified - missing PAYE scheme",
                _ => "Not Verified"
            }
        };
    }
}