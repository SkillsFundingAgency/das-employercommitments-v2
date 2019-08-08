using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;

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
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>());

            return services;
        }
    }
}