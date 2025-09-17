using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelMapper : IMapper<AddApprenticeshipCacheModel, ApprenticeViewModel>
{
    private readonly IApprovalsApiClient _client;

    public ApprenticeViewModelMapper(IApprovalsApiClient client)
    {
        _client = client;
    }

    public async Task<ApprenticeViewModel> Map(AddApprenticeshipCacheModel source)
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
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            DeliveryModel = source.DeliveryModel,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email,
            BirthDay = source.BirthDay,
            BirthMonth = source.BirthMonth,
            BirthYear = source.BirthYear,
            StartMonth = source.StartMonth,
            StartYear = source.StartYear,
            EndMonth = source.EndMonth,
            EndYear = source.EndYear,
            EmploymentEndMonth = source.EmploymentEndMonth,
            EmploymentEndYear = source.EmploymentEndYear,
            Cost = source.Cost,
            EmploymentPrice = source.EmploymentPrice,
            Reference = source.Reference,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey,
            StandardPageUrl = details.StandardPageUrl,
            FundingBandMax = details.ProposedMaxFunding
        };

        return result;
    }
}