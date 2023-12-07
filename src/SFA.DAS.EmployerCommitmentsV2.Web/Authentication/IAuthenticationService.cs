using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

public interface IAuthenticationService
{
    bool IsUserAuthenticated();
    bool TryGetUserClaimValue(string key, out string value);
    string UserName { get; }
    string UserId { get; }
    string UserEmail { get; }
    UserInfo UserInfo { get; }
}