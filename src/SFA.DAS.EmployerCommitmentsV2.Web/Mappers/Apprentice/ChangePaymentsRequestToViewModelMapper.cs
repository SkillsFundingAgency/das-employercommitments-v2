using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangePaymentsRequestToViewModelMapper(
    IApprovalsApiClient approvalsApiClient,
    ICurrentDateTime currentDateTime)
    : IMapper<ChangePaymentsRequest, ChangePaymentsRequestViewModel>
{
    public async Task<ChangePaymentsRequestViewModel> Map(ChangePaymentsRequest source)
    {
        var details = await approvalsApiClient.GetChangePayments(source.AccountId, source.ApprenticeshipId);
        var today = currentDateTime.UtcNow.Date;
        var paymentsFrozen = details.FreezeStatus;

        return new ChangePaymentsRequestViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeshipId = source.ApprenticeshipId,
            AccountId = source.AccountId,
            ApprenticeName = $"{details.FirstName} {details.LastName}",
            ULN = details.Uln,
            Course = details.CourseName,
            FreezeStatus = paymentsFrozen,
            PauseDate = paymentsFrozen ? details.PaymentFreezeDate!.Value.Date : today,
            ResumeDate = paymentsFrozen ? today : null
        };
    }
}
