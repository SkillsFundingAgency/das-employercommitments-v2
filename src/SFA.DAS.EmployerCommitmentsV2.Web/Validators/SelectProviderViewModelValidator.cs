using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SelectProviderViewModelValidator : AbstractValidator<SelectProviderViewModel>
{
    public SelectProviderViewModelValidator()
    {
        RuleFor(x => x.ProviderId)
            .Must(BeALongGreaterThanZero)
            .WithMessage("Check UK Provider Reference Number");

        RuleFor(x => x.AccountLegalEntityHashedId)
            .NotNull()
            .NotEmpty();
    }

    private static bool BeALongGreaterThanZero(string value)
    {
        if (long.TryParse(value, out var providerId))
        {
            return providerId > 0;
        }
        return false;
    }
}