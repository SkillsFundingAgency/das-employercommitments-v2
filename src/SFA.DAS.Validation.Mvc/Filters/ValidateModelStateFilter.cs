using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Validation.Exceptions;
using SFA.DAS.Validation.Mvc.Extensions;
using SFA.DAS.Validation.Mvc.ModelBinding;

namespace SFA.DAS.Validation.Mvc.Filters;

public class ValidateModelStateFilter(ILogger<ValidateModelStateFilter> logger) : ActionFilterAttribute
{
    private static readonly string ModelStateKey = typeof(SerializableModelStateDictionary).FullName;

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.HttpContext.Request.Method == "GET")
        {
            if (!filterContext.ModelState.IsValid)
            {
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
            }
            else
            {
                var tempDataFactory = filterContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
                var tempData = tempDataFactory.GetTempData(filterContext.HttpContext);
                var serializableModelState = tempData.Get<SerializableModelStateDictionary>();
                var modelState = serializableModelState?.ToModelState();
                    
                filterContext.ModelState.Merge(modelState);
            }
        }
        else if (!filterContext.ModelState.IsValid)
        {
            var tempDataFactory = filterContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(filterContext.HttpContext);
            var serializableModelState = filterContext.ModelState.ToSerializable();
            
            logger.LogInformation("ValidateModelStateFilter.OnActionExecuting tempData before saving: {Model}", JsonConvert.SerializeObject(tempData));
            
            logger.LogInformation("ValidateModelStateFilter.OnActionExecuting serializableModelState: {Model}", JsonConvert.SerializeObject(serializableModelState));
                
            tempData.Set(serializableModelState);
            
            logger.LogInformation("ValidateModelStateFilter.OnActionExecuting tempData after saving: {Model}", JsonConvert.SerializeObject(tempData));
            
            filterContext.RouteData.Values.Merge(filterContext.HttpContext.Request.Query);
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.HttpContext.Request.Method != "GET")
        {
            if (filterContext.Exception is ValidationException validationException)
            {
                filterContext.ModelState.AddModelError(validationException);
                    
                var tempDataFactory = filterContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
                var tempData = tempDataFactory.GetTempData(filterContext.HttpContext);
                var serializableModelState = filterContext.ModelState.ToSerializable();
                
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing validation exception, tempData before saving: {Model}", JsonConvert.SerializeObject(tempData));
                
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing validation exception, serializableModelState: {Model}", JsonConvert.SerializeObject(serializableModelState));
                    
                tempData.Set(serializableModelState);
                
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing validation exception, tempData after saving: {Model}", JsonConvert.SerializeObject(tempData));
                
                filterContext.RouteData.Values.Merge(filterContext.HttpContext.Request.Query);
                filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
                filterContext.ExceptionHandled = true;
            }
            else if (!filterContext.ModelState.IsValid)
            {
                var tempDataFactory = filterContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
                var tempData = tempDataFactory.GetTempData(filterContext.HttpContext);
                var serializableModelState = filterContext.ModelState.ToSerializable();
                    
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing !filterContext.ModelState.IsValid, tempData before saving: {Model}", JsonConvert.SerializeObject(tempData));
                
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing !filterContext.ModelState.IsValid, serializableModelState: {Model}", JsonConvert.SerializeObject(serializableModelState));
                    
                tempData.Set(serializableModelState);
                
                logger.LogInformation("ValidateModelStateFilter.OnActionExecuted processing !filterContext.ModelState.IsValid, tempData after saving: {Model}", JsonConvert.SerializeObject(tempData));

            }
        }
    }
}