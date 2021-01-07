using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhatIsTheNewStartDateViewModelValidator : AbstractValidator<WhatIsTheNewStartDateViewModel>
    {
        public WhatIsTheNewStartDateViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.NewStartDate).Must(y => y.Month.HasValue && y.Year.HasValue).WithMessage("Enter the start date with the new training provider");
            RuleFor(r => r.NewStartDate).Must(y => y.Year.HasValue).WithMessage("The start date must include a year");
            RuleFor(r => r.NewStartDate).Must(y => y.Month.HasValue).WithMessage("The start date must include a month");
            RuleFor(x => x.NewStartDate).Must(y => y.IsValid).WithMessage($"The start date must be a real date").When(z => z.NewStartDate.HasValue);

        }
    }
}
