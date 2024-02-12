using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class AccessDraftApprenticeshipAuthorizationHandler(ICommitmentsAuthorisationHandler handler) : AuthorizationHandler<AccessDraftApprenticeshipRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessDraftApprenticeshipRequirement requirement)
    {
        if (!await handler.CanAccessCohort())
        {
            return;
        }

        context.Succeed(requirement);
    }
}