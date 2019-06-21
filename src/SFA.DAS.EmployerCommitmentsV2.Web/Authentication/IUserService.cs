namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public interface IUserService
    {
        bool IsUserAuthenticated();
        bool TryGetUserClaimValue(string key, out string value);
    }
}