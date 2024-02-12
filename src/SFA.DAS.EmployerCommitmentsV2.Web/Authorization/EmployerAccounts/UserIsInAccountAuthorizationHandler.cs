using Microsoft.AspNetCore.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Requirements;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class UserIsInAccountAuthorizationHandler : AuthorizationHandler<UserIsInAccountRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _handler;

    public UserIsInAccountAuthorizationHandler(IEmployerAccountAuthorisationHandler handler)
    {
        _handler = handler;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsInAccountRequirement ownerRequirement)
    {
        if (!await _handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(ownerRequirement);
    }
}