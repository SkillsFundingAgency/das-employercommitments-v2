using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Validation.Mvc.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Startup
{
    public static class MvcStartup
    {
        public static IServiceCollection AddDasMvc(this IServiceCollection services)
        {
            services.AddMvc(o =>
                {
                    o.AddAuthorization();
                    o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    o.AddValidation();
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .SetDefaultNavigationSection(NavigationSection.ApprenticesHome)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>());

            return services;
        }
    }
}