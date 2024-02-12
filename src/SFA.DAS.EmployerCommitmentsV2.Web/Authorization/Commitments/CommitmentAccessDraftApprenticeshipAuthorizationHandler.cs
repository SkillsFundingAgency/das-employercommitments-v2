using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;

public class CommitmentAccessDraftApprenticeshipAuthorizationHandler : AuthorizationHandler<AccessDraftApprenticeshipRequirement>
{
    private readonly ICommitmentsAuthorisationHandler _handler;

    public CommitmentAccessDraftApprenticeshipAuthorizationHandler(ICommitmentsAuthorisationHandler handler)
    {
        _handler = handler;
    }
    

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessDraftApprenticeshipRequirement requirement)
    {
        if (!await _handler.CanAccessApprenticeship())
        {
            return;
        }

        context.Succeed(requirement);
    }
}