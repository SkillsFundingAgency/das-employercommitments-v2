using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhatIsTheNewEndDateViewModelValidator : AbstractValidator<WhatIsTheNewEndDateViewModel>
    {
        public WhatIsTheNewEndDateViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.NewEndDate).Must(y => y.Month.HasValue && y.Year.HasValue)
                .WithMessage("Enter the new planned training end date with the new training provider");
            RuleFor(r => r.NewEndDate).Must(y => y.Year.HasValue)
                .WithMessage("The new planned training end date must include a year");
            RuleFor(r => r.NewEndDate).Must(y => y.Month.HasValue)
                .WithMessage("The new planned training end date must include a month");
            RuleFor(r => r.NewEndDate).Must(y => y.IsValid)
                .WithMessage("The new planned training end date must be a real date").When(z => z.NewEndDate.HasValue);
            RuleFor(r => r.NewEndDate).Must((args, value) => value.Date != null && value.Date.Value > args.NewStartDate)
                .WithMessage(m => $"The new planned training end date must be after {m.NewStartDate:MMMM yyyy}")
                .When(z => z.NewEndDate.IsValid);
        }
    }
}
