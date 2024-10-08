﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.EmployerAccounts;
using SFA.DAS.GovUK.Auth.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class AuthorizationServiceRegistrations
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
    {
        BearerTokenProvider.SetSigningKey(configuration["SFA.DAS.EmployerCommitmentsV2:UserBearerTokenSigningKey"]);

        services.AddSingleton<CommitmentsAuthorisationHandler>();
        services.AddSingleton<ICommitmentsAuthorisationHandler, CommitmentsAuthorisationHandler>();
        services.AddTransient<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        services.AddSingleton(serviceProvider => serviceProvider.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient());

        services.AddSingleton<IEmployerAccountAuthorisationHandler, EmployerAccountAuthorisationHandler>();

        services.AddSingleton<IAuthorizationHandler, AccessApprenticeshipAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, AccessCohortAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, EmployerTransactorOwnerAccountAuthorizationHandler>();

        services.AddTransient<IAuthorizationContext, AuthorizationContext>();
        services.AddSingleton<IAuthorizationContextProvider, AuthorizationContextProvider>();

        AddAuthorizationPolicies(services);

        return services;
    }
    
    private static void AddAuthorizationPolicies(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            
            options.AddPolicy(PolicyNames.HasActiveAccount, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new UserIsInAccountRequirement());
                policy.RequireAuthenticatedUser();
            });
            
            options.AddPolicy(PolicyNames.HasEmployerTransactorOwnerAccount, policy =>
            {
                policy.RequireClaim(EmployerClaims.AccountsClaimsTypeIdentifier);
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new EmployerTransactorOwnerAccountRequirement());
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(PolicyNames.AccessApprenticeship, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessApprenticeshipRequirement());
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(PolicyNames.AccessDraftApprenticeship, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessCohortRequirement());
                policy.Requirements.Add(new EmployerTransactorOwnerAccountRequirement());
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(PolicyNames.AccessCohort, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessCohortRequirement());
                policy.RequireAuthenticatedUser();
            });
        });
    }
}