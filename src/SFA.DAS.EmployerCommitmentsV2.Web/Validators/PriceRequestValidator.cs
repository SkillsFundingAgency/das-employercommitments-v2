using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.CommitmentsV2.Shared.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class PriceRequestValidator : AbstractValidator<EmployerLedChangeOfProviderRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(x => x.ProviderId).GreaterThan(0);
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.AccountHashedId).NotEmpty();
            RuleFor(x => x.NewStartDate).Must(field => field.IsValidMonthYear());
            RuleFor(x => x.NewEndDate).Must(field => field.IsValidMonthYear());
        }
    }
}
