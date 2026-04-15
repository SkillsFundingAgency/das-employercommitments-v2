using System.ComponentModel.DataAnnotations;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmStopRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient,
    IEncodingService encodingService) : IMapper<ConfirmStopRequest, ConfirmStopRequestViewModel>
{   

    public async Task<ConfirmStopRequestViewModel> Map(ConfirmStopRequest source)
    {
        var accountId = encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var apprenticeshipdetails = await approvalsApiClient.GetManageApprenticeshipDetails(accountId, source.ApprenticeshipId, cancellationToken: CancellationToken.None);
        var apprenticeship = apprenticeshipdetails.Apprenticeship;
        var stoppedDate = GetStoppedDate(source, apprenticeship);

        return new ConfirmStopRequestViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            IsCoPJourney = source.IsCoPJourney,
            StopMonth = source.StopMonth,
            StopYear = source.StopYear,
            ApprenticeName = $"{apprenticeship.FirstName} {apprenticeship.LastName}",
            ULN = apprenticeship.Uln,
            Course = apprenticeship.CourseName,
            StopDate = stoppedDate,
            MadeRedundant = source.MadeRedundant,
            LearningType = apprenticeship.LearningType.FormatEnumValue()
        };
    }

    private static DateTime GetStoppedDate(ConfirmStopRequest source, GetApprenticeshipResponse apprenticeship)
    {
        return apprenticeship.Status == CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart 
            ? apprenticeship.StartDate.Value 
            : new DateTime(source.StopYear.Value, source.StopMonth.Value, 1);
    }
}