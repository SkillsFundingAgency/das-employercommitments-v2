using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelToApprenticeRequestMapper : IMapper<ApprenticeViewModel, ApprenticeRequest>
{
    public Task<ApprenticeRequest> Map(ApprenticeViewModel source)
    {
        return Task.FromResult(new ApprenticeRequest
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = source.LegalEntityName,                
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            ProviderId = (int)source.ProviderId,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Origin = source.Origin,
            DeliveryModel = source.DeliveryModel,
            CacheKey = source.CacheKey
        });
    }
}