﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAuthorisationHandler : IEmployerAccountAuthorisationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAccountService _accountsService;
    private readonly ILogger<EmployerAccountAuthorisationHandler> _logger;
    
    // To allow unit testing
    public int MaxPermittedNumberOfAccountsOnClaim { get; set; } = WebConstants.MaxNumberOfEmployerAccountsAllowedOnClaim;

    public EmployerAccountAuthorisationHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserAccountService accountsService,
        ILogger<EmployerAccountAuthorisationHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountsService = accountsService;
        _logger = logger;
    }
    
    public async Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole minimumAllowedRole)
    {
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }

        var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
        var employerAccountsClaim = context.User.FindFirst(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));

        Dictionary<string, EmployerUserAccountItem> employerAccounts = null;

        if (employerAccountsClaim != null)
        {
            try
            {
                employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountsClaim.Value);
            }
            catch (JsonSerializationException e)
            {
                _logger.LogError(e, "Could not deserialize employer account claim for user");
                return false;
            }
        }

        EmployerUserAccountItem employerIdentifier = null;

        if (employerAccounts != null)
        {
            employerIdentifier = employerAccounts.TryGetValue(accountIdFromUrl, out var account)
                ? account
                : null;
        }

        if (employerAccounts == null || !employerAccounts.ContainsKey(accountIdFromUrl))
        {
            if (!context.User.HasClaim(c => c.Type.Equals(ClaimTypes.NameIdentifier)))
            {
                return false;
            }

            var userClaim = context.User.Claims
                .First(c => c.Type.Equals(ClaimTypes.NameIdentifier));

            var email = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
            var userId = userClaim.Value;

            var result = await _accountsService.GetUserAccounts(userId, email);

            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployeeClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);
            var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(associatedAccountsClaim.Value);

            // Some users have 100's of employer accounts. The claims cannot handle that volume of data.
            if (updatedEmployerAccounts.Count <= MaxPermittedNumberOfAccountsOnClaim)
            {
                userClaim.Subject.AddClaim(associatedAccountsClaim);
            }

            if (!updatedEmployerAccounts.ContainsKey(accountIdFromUrl))
            {
                return false;
            }

            employerIdentifier = updatedEmployerAccounts[accountIdFromUrl];
        }

        if (!_httpContextAccessor.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
        {
            _httpContextAccessor.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
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