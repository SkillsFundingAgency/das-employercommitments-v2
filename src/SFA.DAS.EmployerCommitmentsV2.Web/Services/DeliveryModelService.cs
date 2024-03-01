using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Services;

public interface IDeliveryModelService
{
    Task<bool> HasMultipleDeliveryModels(long providerId, string courseCode, string accountLegalEntity);
}

public class DeliveryModelService : IDeliveryModelService
{
    private readonly IEncodingService _encodingService;
    private readonly IApprovalsApiClient _approvalsApiClient;

    public DeliveryModelService(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    {
        _encodingService = encodingService;
        _approvalsApiClient = approvalsApiClient;
    }

    public async Task<bool> HasMultipleDeliveryModels(long providerId, string courseCode, string accountLegalEntity)
    {
        var aleId = _encodingService.Decode(accountLegalEntity, EncodingType.PublicAccountLegalEntityId);
        var response = await _approvalsApiClient.GetProviderCourseDeliveryModels(providerId, courseCode, aleId);
        return response?.DeliveryModels.Count() > 1;
    }
}