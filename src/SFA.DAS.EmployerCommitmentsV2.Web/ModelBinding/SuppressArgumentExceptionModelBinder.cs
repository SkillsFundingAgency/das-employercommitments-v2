using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SFA.DAS.EmployerCommitmentsV2.Attributes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;

public class SuppressArgumentExceptionModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
        var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);
        try
        {
            var result = converter.ConvertFrom(valueResult.FirstValue);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (ArgumentException)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            var errorSuppressBinderAttribute = (bindingContext.ModelMetadata as DefaultModelMetadata).Attributes.PropertyAttributes.FirstOrDefault(x => x.GetType() == typeof(SuppressArgumentExceptionAttribute)) as SuppressArgumentExceptionAttribute;
            bindingContext.ModelState.TryAddModelError(
                errorSuppressBinderAttribute.PropertyName,
                errorSuppressBinderAttribute.CustomErrorMessage);
        }

        return Task.CompletedTask;
    }
}