using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAllRolesAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<EmployerTransactorOwnerAccountRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerTransactorOwnerAccountRequirement ownerRequirement)
    {
        if (!await handler.IsEmployerAuthorised(context, EmployerUserRole.Viewer))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}