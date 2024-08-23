using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Services;

public interface IReservationsService
{
    Task<GetAccountReservationsStatusResponse> GetAccountReservationsStatus(long accountId, long? transferSenderId);
}

public class ReservationsService : IReservationsService
{
    private readonly IApprovalsApiClient _outerApiClient;

    public ReservationsService(IApprovalsApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }
    public Task<GetAccountReservationsStatusResponse> GetAccountReservationsStatus(long accountId, long? transferSenderId)
    {
        return _outerApiClient.GetAccountReservationsStatus(accountId, transferSenderId);
    }
}
