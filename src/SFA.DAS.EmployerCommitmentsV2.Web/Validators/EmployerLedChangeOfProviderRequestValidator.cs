using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EmployerLedChangeOfProviderRequestValidator : AbstractValidator<EmployerLedChangeOfProviderRequest>
    {
        public EmployerLedChangeOfProviderRequestValidator()
        {
            RuleFor(x => x.AccountHashedId).NotEmpty();
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();

            RuleFor(x => x.ProviderId).GreaterThan(0);
        }
    }
}
