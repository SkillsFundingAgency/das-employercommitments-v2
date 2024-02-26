using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public class AccessDraftApprenticeshipAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAuthorisationHandler, ICommitmentsAuthorisationHandler commitmentsAuthorisationHandler) : AuthorizationHandler<AccessDraftApprenticeshipRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessDraftApprenticeshipRequirement ownerRequirement)
    {
        var isEmployerAuthorisedTask = employerAuthorisationHandler.IsEmployerAuthorised(context, true);
        var canAccessCohortTask = commitmentsAuthorisationHandler.CanAccessCohort();

        await Task.WhenAll(isEmployerAuthorisedTask, canAccessCohortTask);
        
        if (!isEmployerAuthorisedTask.Result)
        {
            return;
        }
        
        if (!canAccessCohortTask.Result)
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}