using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

namespace SFA.DAS.EmployerCommitmentsV2.Services;

public interface IUserAccountService
{
    Task<EmployerUserAccounts> GetUserAccounts(string userId, string email);
}

public class UserAccountService : IUserAccountService
{
    private readonly IApprovalsApiClient _outerApiClient;

    public UserAccountService(IApprovalsApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }
    public async Task<EmployerUserAccounts> GetUserAccounts(string userId, string email)
    {
        var actual = await _outerApiClient.GetEmployerUserAccounts(email, userId);

        return actual;
    }
}
