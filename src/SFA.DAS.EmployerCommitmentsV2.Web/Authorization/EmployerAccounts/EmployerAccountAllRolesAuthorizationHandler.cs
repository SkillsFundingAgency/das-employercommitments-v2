using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAllRolesAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<EmployerAccountAllRolesRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _handler = handler;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountAllRolesRequirement ownerAllRolesRequirement)
    {
        if (!await _handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(ownerAllRolesRequirement);
    }
}