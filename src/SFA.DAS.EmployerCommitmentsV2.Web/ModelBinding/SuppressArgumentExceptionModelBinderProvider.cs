﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SFA.DAS.EmployerCommitmentsV2.Attributes;

namespace SFA.DAS.EmployerCommitmentsV2.Web.ModelBinding;

public class SuppressArgumentExceptionModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext bindingContext)
    {
        var isArgumentExcpSuppressModelBoundProperty = (bindingContext.Metadata as DefaultModelMetadata)?.Attributes?.PropertyAttributes?.Where(x => x.GetType() == typeof(SuppressArgumentExceptionAttribute))?.Count() > 0;
        if (isArgumentExcpSuppressModelBoundProperty)
        {
            var errorSuppressModelBinder = new SuppressArgumentExceptionModelBinder();
            return errorSuppressModelBinder;
        }

        return null;
    }
}