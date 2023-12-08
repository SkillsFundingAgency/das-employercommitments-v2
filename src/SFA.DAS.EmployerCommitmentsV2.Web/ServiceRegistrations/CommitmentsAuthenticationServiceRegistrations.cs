using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Client;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization.Commitments;
using SFA.DAS.GovUK.Auth.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class CommitmentsAuthenticationServiceRegistrations
{
    public static IServiceCollection AddCommitmentsAuthorization(this IServiceCollection services)
    {
        services
            .AddScoped<CommitmentsAuthorisationHandler>()
            .AddSingleton(serviceProvider => serviceProvider.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient())
            .AddTransient<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.AccessCohort, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessCohortRequirement());
                policy.RequireAuthenticatedUser();
            });
            
            options.AddPolicy(PolicyNames.AccessApprenticeship, policy =>
            {
                policy.Requirements.Add(new AccountActiveRequirement());
                policy.Requirements.Add(new AccessApprenticeshipRequirement());
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }
}