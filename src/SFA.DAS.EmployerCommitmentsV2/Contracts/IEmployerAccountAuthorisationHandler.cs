using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface IEmployerAccountAuthorisationHandler
{
    Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles);
    Task<bool> IsOutsideAccount(AuthorizationHandlerContext context);
}