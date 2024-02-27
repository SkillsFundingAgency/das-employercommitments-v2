using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerTransactorOwnerAccountAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAuthorisationHandler) : AuthorizationHandler<EmployerTransactorOwnerAccountRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerTransactorOwnerAccountRequirement ownerRequirement)
    {
        if (!await employerAuthorisationHandler.IsEmployerAuthorised(context, EmployerUserRole.Transactor))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}