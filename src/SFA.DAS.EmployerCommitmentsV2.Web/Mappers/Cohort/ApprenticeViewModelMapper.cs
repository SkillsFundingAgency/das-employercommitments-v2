using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelMapper : IMapper<ApprenticeRequest, ApprenticeViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;

    public ApprenticeViewModelMapper(ICommitmentsApiClient commitmentsApiClient)
    {
        _commitmentsApiClient = commitmentsApiClient;
    }

    public async Task<ApprenticeViewModel> Map(ApprenticeRequest source)
    {
        var accountLegalEntity = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var provider = await _commitmentsApiClient.GetProvider(source.ProviderId);

        var result = new ApprenticeViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = accountLegalEntity.LegalEntityName,
            StartDate = new MonthYearModel(source.StartMonthYear),
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            ProviderId = (int)source.ProviderId,
            ProviderName = provider.Name,
            Courses = null,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Origin = source.Origin,
            AutoCreatedReservation = source.AutoCreated,
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false,
            CacheKey = source.CacheKey.IsNotNullOrEmpty() ? source.CacheKey : Guid.NewGuid()
        };

        return result;
    }
}