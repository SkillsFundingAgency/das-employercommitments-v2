using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAllRoleAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<EmployerAccountAllRolesRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountAllRolesRequirement ownerRequirement)
    {
        if (!await handler.IsEmployerAuthorised(context, EmployerUserRole.All))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}