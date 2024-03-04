using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface IEmployerAccountAuthorisationHandler
{
    Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole minimumAllowedRole);
}
