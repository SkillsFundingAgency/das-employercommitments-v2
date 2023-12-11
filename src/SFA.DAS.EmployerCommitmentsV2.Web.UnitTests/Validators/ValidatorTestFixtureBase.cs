using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators;

public class ValidatorTestBase<TInstance, TValidator> where TValidator : AbstractValidator<TInstance>
{
    protected virtual TValidator ValidatorInitialize()
    {
        return (TValidator)Activator.CreateInstance(typeof(TValidator));
    }
        
    protected void AssertValidationResult<T>(Expression<Func<TInstance, T>> property, TInstance instance, bool isValid, string expectedErrorMessage = null)
    {
        TValidator validator = ValidatorInitialize();
        var validationResult = validator.TestValidate(instance);

        if (isValid)
        {

            validationResult.ShouldNotHaveValidationErrorFor<T>(property);
        }
        else
        {
            if (string.IsNullOrEmpty(expectedErrorMessage))
            {
                validationResult.ShouldHaveValidationErrorFor(property);
            }
            else
            {
                validationResult.ShouldHaveValidationErrorFor(property).WithErrorMessage(expectedErrorMessage);
            }
                
        }
    }
}