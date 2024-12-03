using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.EmployerCommitmentsV2.Services;

public class UserAccountService(IApprovalsApiClient outerApiClient) : IGovAuthEmployerAccountService
{
    async Task<EmployerUserAccounts> IGovAuthEmployerAccountService.GetUserAccounts(string userId, string email)
    {
        var result = await outerApiClient.GetEmployerUserAccounts(email, userId);

        return new EmployerUserAccounts
        {
            EmployerAccounts = result.UserAccounts != null? result.UserAccounts.Select(c => new EmployerUserAccountItem
            {
                Role = c.Role,
                AccountId = c.AccountId,
                ApprenticeshipEmployerType = Enum.Parse<ApprenticeshipEmployerType>(c.ApprenticeshipEmployerType.ToString()),
                EmployerName = c.EmployerName,
            }).ToList() : [],
            FirstName = result.FirstName,
            IsSuspended = result.IsSuspended,
            LastName = result.LastName,
            EmployerUserId = result.EmployerUserId,
        };
    }
}