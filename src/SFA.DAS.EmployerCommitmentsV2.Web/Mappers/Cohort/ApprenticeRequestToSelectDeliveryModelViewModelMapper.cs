using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeRequestToSelectDeliveryModelViewModelMapper : IMapper<ApprenticeRequest, SelectDeliveryModelViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;

    public ApprenticeRequestToSelectDeliveryModelViewModelMapper(IApprovalsApiClient approvalsApiClient)
        => (_approvalsApiClient) = (approvalsApiClient);

    public async Task<SelectDeliveryModelViewModel> Map(ApprenticeRequest source)
    {
        var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(source.ProviderId, source.CourseCode, source.AccountLegalEntityId);

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
            CacheKey = source.CacheKey
        };
    }
}