using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;

public interface IAuthorizationContextProvider
{
    IAuthorizationContext GetAuthorizationContext();
}