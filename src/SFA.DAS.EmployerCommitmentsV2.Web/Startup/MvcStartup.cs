using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services, IConfiguration configuration)
        {
            var commitmentsConfiguration = configuration.GetSection(ConfigurationKeys.EmployerCommitmentsV2)
                .Get<EmployerCommitmentsV2Configuration>();
            
            services.AddMvc(o =>
                {
                    o.EnableEndpointRouting = false;
                    o.AddAuthorization();
                    o.AddValidation();
                    o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    o.Filters.Add(new GoogleAnalyticsFilter());
                    o.ModelBinderProviders.Insert(0, new SuppressArgumentExceptionModelBinderProvider());
                    o.Filters.Add(new AccountActiveFilter(configuration));
                    if (commitmentsConfiguration.UseGovSignIn)
                    {
                        o.Filters.Add(new AuthorizeFilter("HasActiveAccount"));    
                    }
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .SetDefaultNavigationSection(NavigationSection.ApprenticesHome)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>());

            return services;
        }
    }
}