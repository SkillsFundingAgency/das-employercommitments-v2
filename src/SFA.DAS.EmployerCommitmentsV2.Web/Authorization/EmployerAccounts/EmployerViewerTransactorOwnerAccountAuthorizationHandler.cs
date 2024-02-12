using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerViewerTransactorOwnerAccountAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<EmployerViewerTransactorOwnerAccountRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerViewerTransactorOwnerAccountRequirement ownerRequirement)
    {
        if (!await handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}