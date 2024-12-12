using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class AddApprenticeshipCacheModelToSelectDeliveryModelViewModelMapper(IApprovalsApiClient approvalsApiClient) : IMapper<AddApprenticeshipCacheModel, SelectDeliveryModelViewModel>
{
    public async Task<SelectDeliveryModelViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var response = await approvalsApiClient.GetProviderCourseDeliveryModels(source.ProviderId, source.CourseCode, source.AccountLegalEntityId);

        return new SelectDeliveryModelViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            CourseCode = source.CourseCode,
            DeliveryModel = source.DeliveryModel,
            DeliveryModels = response.DeliveryModels.ToArray(),
            ProviderId = source.ProviderId,
            ReservationId = source.ReservationId,
            StartMonthYear = source.StartMonthYear,
            TransferSenderId = source.TransferSenderId,
            CacheKey = source.AddApprenticeshipCacheKey,
            AddApprenticeshipCacheKey = source.AddApprenticeshipCacheKey
        };
    }
}