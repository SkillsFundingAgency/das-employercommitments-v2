using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmProviderRequestValidator : AbstractValidator<ConfirmProviderRequest>
{
    public ConfirmProviderRequestValidator()
    {
        RuleFor(x => x.ProviderId).GreaterThan(0);
    }
}