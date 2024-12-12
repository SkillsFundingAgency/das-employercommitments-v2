using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AssignViewModelMapper(ICommitmentsApiClient commitmentsApiClient) : IMapper<AddApprenticeshipCacheModel, AssignViewModel>
{
    public async Task<AssignViewModel> Map(AddApprenticeshipCacheModel request)
    {
        var accountLegalEntity = await commitmentsApiClient.GetAccountLegalEntity(request.AccountLegalEntityId);

        return new AssignViewModel
        {
            AccountHashedId = request.AccountHashedId,
            AccountLegalEntityHashedId = request.AccountLegalEntityHashedId,
            LegalEntityName = accountLegalEntity.LegalEntityName,
            ReservationId = request.ReservationId,
            StartMonthYear = request.StartMonthYear,
            CourseCode = request.CourseCode,
            ProviderId = request.ProviderId,
            TransferSenderId = request.TransferSenderId,
            EncodedPledgeApplicationId = request.EncodedPledgeApplicationId,
            AddApprenticeshipCacheKey = request.AddApprenticeshipCacheKey
        };
    }
}