using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelMapper(ICommitmentsApiClient commitmentsApiClient) : IMapper<AddApprenticeshipCacheModel, ApprenticeViewModel>
{   
    public async Task<ApprenticeViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var accountLegalEntity = await commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var provider = await commitmentsApiClient.GetProvider(source.ProviderId);

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
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false,
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
            AddApprenticeshipCacheKey = source.CacheKey
        };

        return result;
    }
}