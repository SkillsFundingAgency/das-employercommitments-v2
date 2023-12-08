using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;

public class AuthorizationModelBinder : IModelBinder
{
    private readonly IModelBinder _fallbackModelBinder;

    public AuthorizationModelBinder(IModelBinder fallbackModelBinder)
    {
        _fallbackModelBinder = fallbackModelBinder;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var authorizationContextProvider = bindingContext.HttpContext.RequestServices.GetService<IAuthorizationContextProvider>();
        var authorizationContext = authorizationContextProvider.GetAuthorizationContext();

        if (authorizationContext.TryGet(bindingContext.ModelMetadata.PropertyName, out object value))
        {
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value, value?.ToString());
            bindingContext.Result = ModelBindingResult.Success(value);

            return Task.CompletedTask;
        }

        return _fallbackModelBinder.BindModelAsync(bindingContext);
    }
}