using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class AccessCohortAuthorizationHandler(ICommitmentsAuthorisationHandler handler) : AuthorizationHandler<AccessCohortRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessCohortRequirement requirement)
    {
        //if (!await handler.CanAccessCohort())
        //{
        //    return;
        //}

        context.Succeed(requirement);
    }
}