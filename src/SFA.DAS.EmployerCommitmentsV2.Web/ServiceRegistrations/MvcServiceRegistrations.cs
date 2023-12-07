using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.AppStart;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MvcServiceRegistrations
{
    public static IServiceCollection AddDasMvc(this IServiceCollection services, IConfiguration configuration)
    {
        var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
            .Get<EmployerCommitmentsV2Configuration>();

        services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.AddAuthorization();
                options.AddValidation();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new GoogleAnalyticsFilter());
                options.ModelBinderProviders.Insert(0, new SuppressArgumentExceptionModelBinderProvider());
                options.AddStringModelBinderProvider();
                options.Filters.Add(new AccountActiveFilter(configuration));

                if (commitmentsConfiguration.UseGovSignIn)
                {
                    options.Filters.Add(new AuthorizeFilter("HasActiveAccount"));
                }
            })
            .AddControllersAsServices()
            .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>();

        return services;
    }
}