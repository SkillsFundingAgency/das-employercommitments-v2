using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class CommitmentAccessCohortAuthorizationHandler : AuthorizationHandler<AccessCohortRequirement>
{
    private readonly ICommitmentsAuthorisationHandler _handler;

    public CommitmentAccessCohortAuthorizationHandler(ICommitmentsAuthorisationHandler handler)
    {
        _handler = handler;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessCohortRequirement requirement)
    {
        if (!await _handler.CanAccessCohort())
        {
            return;
        }

        context.Succeed(requirement);
    }
}