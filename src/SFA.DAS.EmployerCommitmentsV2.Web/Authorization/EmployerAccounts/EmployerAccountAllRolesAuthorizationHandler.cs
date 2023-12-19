using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAllRolesAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<EmployerAccountAllRolesRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountAllRolesRequirement ownerAllRolesRequirement)
    {
        if (!await handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(ownerAllRolesRequirement);
    }
}