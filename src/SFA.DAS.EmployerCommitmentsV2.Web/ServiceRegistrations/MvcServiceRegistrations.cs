using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MvcServiceRegistrations
{
    public static IServiceCollection AddDasMvc(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();

        services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.AddValidation();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new GoogleAnalyticsFilter());
                options.ModelBinderProviders.Insert(0, new AuthorizationModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new SuppressArgumentExceptionModelBinderProvider());
                options.AddStringModelBinderProvider();

                if (commitmentsConfiguration.UseGovSignIn)
                {
                    options.Filters.Add(new AuthorizeFilter(PolicyNames.HasActiveAccount));
                }
            })
            .AddControllersAsServices()
            .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>();

        return services;
    }
}