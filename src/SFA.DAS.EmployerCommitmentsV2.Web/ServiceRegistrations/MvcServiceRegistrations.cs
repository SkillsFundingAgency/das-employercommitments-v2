﻿using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SFA.DAS.CommitmentsV2.Shared.Filters;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.EmployerCommitmentsV2.Web.Filters;
using SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Validation.Mvc.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ServiceRegistrations;

public static class MvcServiceRegistrations
{
    public static IServiceCollection AddDasMvc(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.AddValidation();
                options.Filters.Add<DomainExceptionRedirectGetFilterAttribute>();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new GoogleAnalyticsFilter());
                options.ModelBinderProviders.Insert(0, new AuthorizationModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new SuppressArgumentExceptionModelBinderProvider());
            })
            .AddControllersAsServices()
            .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>();

        return services;
    }
}