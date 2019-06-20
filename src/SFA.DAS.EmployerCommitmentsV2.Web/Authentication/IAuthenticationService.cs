namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public interface IAuthenticationService
    {
        bool IsUserAuthenticated();
        bool TryGetUserClaimValue(string key, out string value);
    }
}