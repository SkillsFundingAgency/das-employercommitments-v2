using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class WhatIsTheNewEndDateViewModelValidator : AbstractValidator<WhatIsTheNewEndDateViewModel>
{
    public WhatIsTheNewEndDateViewModelValidator()
    {
        RuleFor(r => r.AccountHashedId).NotEmpty();

        RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();

        RuleFor(r => r.NewEndDate)
            .Must((r, newEndDate) => r.NewEndDate.HasValue && r.NewEndYear.HasValue)
            .WithMessage("Enter the new planned training end date with the new training provider")
            .Unless(r => r.NewEndYear.HasValue || r.NewEndMonth.HasValue);

        RuleFor(r => r.NewEndDate)
            .Must(y => y.Year.HasValue)
            .WithMessage("The new planned training end date must include a year")
            .When(r => r.NewEndMonth.HasValue);

        RuleFor(r => r.NewEndDate)
            .Must(y => y.Month.HasValue)
            .WithMessage("The new planned training end date must include a month")
            .When(r => r.NewEndYear.HasValue);

        RuleFor(x => x.NewEndDate)
            .Must(y => y.IsValid)
            .WithMessage("The new planned training end date must be a real date")
            .When(z => z.NewEndMonth.HasValue && z.NewEndYear.HasValue);

        RuleFor(r => r.NewEndDate)
            .Must((args, value) => value.Date != null && value.Date.Value > args.NewStartDate)
            .WithMessage(m => $"The new planned training end date must be after {m.NewStartDate:MMMM yyyy}")
            .When(z => z.NewEndDate.IsValid);
    }
}