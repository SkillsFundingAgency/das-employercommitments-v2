using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerTransactorOwnerAccountAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAuthorisationHandler) : AuthorizationHandler<AccessDraftApprenticeshipRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessDraftApprenticeshipRequirement ownerRequirement)
    {
        if (!await employerAuthorisationHandler.IsEmployerAuthorised(context, EmployerUserRole.Transactor))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}