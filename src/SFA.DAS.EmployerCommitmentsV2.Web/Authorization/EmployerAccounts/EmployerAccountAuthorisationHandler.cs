﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using JsonClaimValueTypes = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;

public class EmployerAccountAuthorisationHandler : IEmployerAccountAuthorisationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAccountService _accountsService;
    private readonly ILogger<EmployerAccountAuthorisationHandler> _logger;
    private readonly EmployerCommitmentsV2Configuration _configuration;

    public EmployerAccountAuthorisationHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserAccountService accountsService,
        ILogger<EmployerAccountAuthorisationHandler> logger,
        EmployerCommitmentsV2Configuration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountsService = accountsService;
        _logger = logger;
        _configuration = configuration;
        _configuration = configuration;
    }

    public async Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole minimumAllowedRole)
    {
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }

        var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
        var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));

        if (employerAccountClaim?.Value == null)
            return false;

        Dictionary<string, EmployerUserAccountItem> employerAccounts;

        try
        {
            employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim.Value);
        }
        catch (JsonSerializationException e)
        {
            _logger.LogError(e, "Could not deserialize employer account claim for user");
            return false;
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
            var requiredIdClaim = _configuration.UseGovSignIn
                ? ClaimTypes.NameIdentifier
                : EmployeeClaims.IdamsUserIdClaimTypeIdentifier;

            if (!context.User.HasClaim(c => c.Type.Equals(requiredIdClaim)))
            {
                return false;
            }

            var userClaim = context.User.Claims
                .First(c => c.Type.Equals(requiredIdClaim));

            var email = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

            var userId = userClaim.Value;

            var result = await _accountsService.GetUserAccounts(userId, email);

            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployeeClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

            var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(associatedAccountsClaim.Value);

            userClaim.Subject.AddClaim(associatedAccountsClaim);

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

    public Task<bool> IsOutsideAccount(AuthorizationHandlerContext context)
    {
        if (_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return Task.FromResult(false);
        }

        var requiredIdClaim = _configuration.UseGovSignIn ? ClaimTypes.NameIdentifier : EmployeeClaims.IdamsUserIdClaimTypeIdentifier;

        if (!context.User.HasClaim(c => c.Type.Equals(requiredIdClaim)))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
    
    private static bool CheckUserRoleForAccess(EmployerUserAccountItem employerIdentifier, EmployerUserRole minimumAllowedRole)
    {
        if (minimumAllowedRole == EmployerUserRole.All)
        {
            return true;
        }
        
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