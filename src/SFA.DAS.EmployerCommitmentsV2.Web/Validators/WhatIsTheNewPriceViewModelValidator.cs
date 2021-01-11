using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhatIsTheNewPriceViewModelValidator : AbstractValidator<WhatIsTheNewPriceViewModel>
    {
        public WhatIsTheNewPriceViewModelValidator()
        {
            RuleFor(x => x.ProviderId).GreaterThan(0);
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.AccountHashedId).NotEmpty();
            RuleFor(x => x.NewStartDate).Must(field => field.IsValid); // TO DO : Check New Start and New End Date
            RuleFor(x => x.NewPrice).NotEmpty().WithMessage("Enter the new agreed apprenticeship price");
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(1).WithMessage("Enter the new agreed apprenticeship price");
            RuleFor(x => x.NewPrice).LessThanOrEqualTo(100000).WithMessage("The new agreed apprenticeship price must be £100,000 or less");
        }
    }
}
