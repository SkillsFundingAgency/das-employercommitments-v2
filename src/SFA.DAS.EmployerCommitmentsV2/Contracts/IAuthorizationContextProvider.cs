namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface IAuthorizationContextProvider
{
    IAuthorizationContext GetAuthorizationContext();
}