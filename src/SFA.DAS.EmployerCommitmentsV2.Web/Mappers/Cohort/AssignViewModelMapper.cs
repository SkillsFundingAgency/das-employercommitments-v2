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
            LegalEntityName = accountLegalEntity.LegalEntityName,
            ReservationId = request.ReservationId,
            ApprenticeshipSessionKey = request.ApprenticeshipSessionKey,
            FundingType = request.FundingType
        };
    }
}