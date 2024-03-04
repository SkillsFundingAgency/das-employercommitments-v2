using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;

public class EmployerUserAccounts
{
    public string Email { get; set; }
    public string EmployerUserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public bool IsSuspended { get; set; }
    public IEnumerable<EmployerUserAccountItem> EmployerAccounts { get; set; }

    public static implicit operator EmployerUserAccounts(GetUserAccountsResponse source)
    {
        var accounts = source?.UserAccounts == null
            ? new List<EmployerUserAccountItem>()
            : source.UserAccounts.Select(c => (EmployerUserAccountItem) c).ToList();
        
        return new EmployerUserAccounts
        {
            FirstName = source?.FirstName,
            LastName = source?.LastName,
            EmployerUserId = source?.EmployerUserId,
            Email = source?.Email,
            EmployerAccounts = accounts,
            IsSuspended = source?.IsSuspended ?? false
        };
    }
}

public class EmployerUserAccountItem
{
    public string AccountId { get; set; }
    public string EmployerName { get; set; }
    public string Role { get; set; }

    public static implicit operator EmployerUserAccountItem(EmployerIdentifier source)
    {
        return new EmployerUserAccountItem
        {
            AccountId = source.AccountId,
            EmployerName = source.EmployerName,
            Role = source.Role
        };
    }
}
