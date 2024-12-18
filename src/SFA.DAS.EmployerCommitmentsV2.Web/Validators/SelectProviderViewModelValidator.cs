using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SelectProviderViewModelValidator : AbstractValidator<SelectProviderViewModel>
{
    public SelectProviderViewModelValidator()
    {
        RuleFor(x => x.ProviderId)
            .Must(BeALongGreaterThanZero)
            .WithMessage("Select a training provider");   
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