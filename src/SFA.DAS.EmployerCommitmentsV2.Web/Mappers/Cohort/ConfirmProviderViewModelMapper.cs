using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ConfirmProviderViewModelMapper(ICommitmentsApiClient commitmentsApiClient) : IMapper<AddApprenticeshipCacheModel, ConfirmProviderViewModel>
{
    public async Task<ConfirmProviderViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var providerResponse = await commitmentsApiClient.GetProvider(source.ProviderId);

        var result = new ConfirmProviderViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = source.LegalEntityName,
            ReservationId = source.ReservationId,
            StartMonthYear = source.StartMonthYear,
            CourseCode = source.CourseCode,
            ProviderId = source.ProviderId,
            ProviderName = providerResponse.Name,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };

        return result;
    }
}