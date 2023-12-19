using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class CommitmentAccessApprenticeshipAuthorizationHandler : AuthorizationHandler<AccessApprenticeshipRequirement>
{
    private readonly ICommitmentsAuthorisationHandler _handler;

    public CommitmentAccessApprenticeshipAuthorizationHandler(ICommitmentsAuthorisationHandler handler)
    {
        _handler = handler;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessApprenticeshipRequirement requirement)
    {
        if (!await _handler.CanAccessApprenticeship())
        {
            return;
        }

        context.Succeed(requirement);
    }
}