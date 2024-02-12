using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class AccessDraftApprenticeshipAuthorizationHandler(IEmployerAccountAuthorisationHandler handler) : AuthorizationHandler<AccessDraftApprenticeshipRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessDraftApprenticeshipRequirement ownerRequirement)
    {
        if (!await handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}