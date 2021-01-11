using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhatIsTheNewStartDateViewModelValidator : AbstractValidator<WhatIsTheNewStartDateViewModel>
    {
        public WhatIsTheNewStartDateViewModelValidator()
        {
            RuleFor(r => r.NewStartDate).Must((r, newStartDate) => r.NewStartMonth.HasValue && r.NewStartYear.HasValue)
                .WithMessage("Enter the start date with the new training provider")
                .Unless(r => r.NewStartYear.HasValue || r.NewStartMonth.HasValue);
                       
            RuleFor(r => r.NewStartDate).Must(y => y.Year.HasValue)
                .WithMessage("The start date must include a year")
                .When(r => r.NewStartMonth.HasValue);
            
            RuleFor(r => r.NewStartDate).Must(y => y.Month.HasValue).WithMessage("The start date must include a month")
                .When(r => r.NewStartYear.HasValue);
            
            RuleFor(x => x.NewStartDate)
                .Must(y => y.IsValid).WithMessage($"The start date must be a real date")
                .When(z => z.NewStartMonth.HasValue && z.NewStartYear.HasValue);
            
            RuleFor(r => r.NewStartDate)
                .Must((r, newStartDate) => newStartDate.IsGreaterThanOrEqualToDateTimeMonthYear(r.StopDate))
                .WithMessage(r => $"The start date must be on or after {r.StopDate:MMMM yyyy}")
                .When(r => r.NewStartDate.IsValid);
        }
    }
}
