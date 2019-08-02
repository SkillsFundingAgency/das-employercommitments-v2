using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Employer.Shared.UI.Startup;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services.AddMvc(o=>o.AddAuthorization())
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .SetDefaultNavigationSection(NavigationSection.ApprenticesHome)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>());

            return services;
        }
    }
}