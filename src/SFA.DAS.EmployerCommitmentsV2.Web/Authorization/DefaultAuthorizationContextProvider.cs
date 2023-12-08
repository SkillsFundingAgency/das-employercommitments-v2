namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public interface IAuthorizationContextProvider
{
    IAuthorizationContext GetAuthorizationContext();
}

public class DefaultAuthorizationContextProvider : IAuthorizationContextProvider
{
    public IAuthorizationContext GetAuthorizationContext()
    {
        return new AuthorizationContext();
    }
}