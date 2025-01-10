using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelMapper : IMapper<ApprenticeRequest, ApprenticeViewModel>
{
    private readonly IApprovalsApiClient _client;

    public ApprenticeViewModelMapper(IApprovalsApiClient client)
    {
        _client = client;
    }

    public async Task<ApprenticeViewModel> Map(ApprenticeRequest source)
    {
        var startDate = new MonthYearModel(source.StartMonthYear);

        var details = await _client.GetAddFirstDraftApprenticeshipDetails(source.AccountId, source.AccountLegalEntityId,
            source.ProviderId, source.CourseCode, startDate.Date);

        var result = new ApprenticeViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = details.LegalEntityName,
            StartDate = startDate,
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            ProviderId = (int)source.ProviderId,
            ProviderName = details.ProviderName,
            Courses = null,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Origin = source.Origin,
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false,
            CacheKey = source.CacheKey.IsNotNullOrEmpty() ? source.CacheKey : Guid.NewGuid(),
            StandardPageUrl = details.StandardPageUrl,
            FundingBandMax = details.ProposedMaxFunding
        };

        return result;
    }
}