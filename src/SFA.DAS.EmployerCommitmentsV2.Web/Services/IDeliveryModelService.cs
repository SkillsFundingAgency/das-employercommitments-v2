namespace SFA.DAS.EmployerCommitmentsV2.Web.Services;

public interface IDeliveryModelService
{
    Task<bool> HasMultipleDeliveryModels(long providerId, string courseCode, string accountLegalEntity);
}