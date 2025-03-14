﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAuthorisationHandler(
    IHttpContextAccessor httpContextAccessor,
    IAccountClaimsService iAccountClaimsService,
    ILogger<EmployerAccountAuthorisationHandler> logger)
    : IEmployerAccountAuthorisationHandler
{
    public async Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole minimumAllowedRole)
    {
        if (!httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }
        
        var user = httpContextAccessor.HttpContext?.User;
        
        if (user.ClaimsAreEmpty())
        {
            return false;
        }

        Dictionary<string, EmployerUserAccountItem> employerAccounts;

        try
        {
            employerAccounts = await iAccountClaimsService.GetAssociatedAccounts(forceRefresh: false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to retrieve employer accounts for user");
            return false;
        }

        EmployerUserAccountItem employerIdentifier = null;

        var accountIdFromUrl = httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();

        if (employerAccounts != null)
        {
            employerIdentifier = employerAccounts.ContainsKey(accountIdFromUrl)
                ? employerAccounts[accountIdFromUrl]
                : null;
        }

        if (!employerAccounts.ContainsKey(accountIdFromUrl))
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ClaimTypes.NameIdentifier)))
            {
                return false;
            }

            var updatedEmployerAccounts = await iAccountClaimsService.GetAssociatedAccounts(forceRefresh: true);

            if (!updatedEmployerAccounts.ContainsKey(accountIdFromUrl))
            {
                return false;
            }

            employerIdentifier = updatedEmployerAccounts[accountIdFromUrl];
        }

        if (!httpContextAccessor.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
        {
            httpContextAccessor.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
        }

        return CheckUserRoleForAccess(employerIdentifier, minimumAllowedRole);
    }

    private static bool CheckUserRoleForAccess(EmployerUserAccountItem employerIdentifier, EmployerUserRole minimumAllowedRole)
    {
        var tryParse = Enum.TryParse<EmployerUserRole>(employerIdentifier.Role, true, out var userRole);

        if (!tryParse)
        {
            return false;
        }

        return minimumAllowedRole switch
        {
            EmployerUserRole.Owner => userRole is EmployerUserRole.Owner,
            EmployerUserRole.Transactor => userRole is EmployerUserRole.Owner or EmployerUserRole.Transactor,
            EmployerUserRole.Viewer => userRole is EmployerUserRole.Owner or EmployerUserRole.Transactor
                or EmployerUserRole.Viewer,
            _ => false
        };
    }
}