using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Customisations;

public class ArrangeActionContextAttribute : CustomizeAttribute
{
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        if (parameter.ParameterType != typeof(ActionExecutingContext))
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        return new ArrangeActionContextCustomisation();
    }

    private class ArrangeActionContextCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new ActionExecutingContextBuilder());
            fixture.Customize<ActionExecutingContext>(composer => composer
                .Without(context => context.Result));
        }
    }

    private class ActionExecutingContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is ParameterInfo paramInfo
                && paramInfo.ParameterType == typeof(object)
                && paramInfo.Name == "controller")
            {
                return context.Create<Controller>();
            }

            return new NoSpecimen();
        }
    }
}