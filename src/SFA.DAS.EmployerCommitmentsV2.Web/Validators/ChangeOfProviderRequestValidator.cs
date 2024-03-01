using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ChangeOfProviderRequestValidator : AbstractValidator<ChangeOfProviderRequest>
{
    public ChangeOfProviderRequestValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();

        RuleFor(x => x.ProviderId).GreaterThan(0);

        RuleFor(x => x.ProviderName).NotEmpty().When(x => x.ProviderName != null);

        RuleFor(x => x.NewStartYear).GreaterThan(2000).When(x => x.NewStartYear != null);

        When(x => x.NewStartMonth != null, () =>
        {
            RuleFor(x => x.NewStartMonth).GreaterThan(0);
            RuleFor(x => x.NewStartMonth).LessThanOrEqualTo(12);
        });

        RuleFor(x => x.NewEndYear).GreaterThan(2000).When(x => x.NewEndYear != null);

        When(x => x.NewEndMonth != null, () =>
        {
            RuleFor(x => x.NewEndMonth).GreaterThan(0);
            RuleFor(x => x.NewEndMonth).LessThanOrEqualTo(12);
        });

        When(x => x.NewPrice != null, () =>
        {
            RuleFor(x => x.NewPrice).GreaterThan(0);
            RuleFor(x => x.NewPrice).LessThanOrEqualTo(100000);
        });
    }
}