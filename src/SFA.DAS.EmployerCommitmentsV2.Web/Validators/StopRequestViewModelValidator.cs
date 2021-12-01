using FluentValidation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class StopRequestViewModelValidator : AbstractValidator<StopRequestViewModel>
    {
        private readonly ICurrentDateTime _currentDateTime;
        public StopRequestViewModelValidator(ICurrentDateTime currentDateTime)
        {
            _currentDateTime = currentDateTime;
            RuleFor(r => r.StopDate).Must((r, StopDate) => r.StopMonth.HasValue && r.StopYear.HasValue)
                 .WithMessage("Enter the stop date for this apprenticeship")
                 .Unless(r => r.StopYear.HasValue || r.StopMonth.HasValue);

            RuleFor(r => r.StopDate).Must(y => y.Year.HasValue)
                .WithMessage("Enter the stop date for this apprenticeship")
                .When(r => r.StopMonth.HasValue);

            RuleFor(r => r.StopDate).Must(y => y.Month.HasValue).WithMessage("Enter the stop date for this apprenticeship")
                .When(r => r.StopYear.HasValue);

            RuleFor(x => x.StopDate)
                .Must(y => y.IsValid).WithMessage($"The stop date must be a real date")
                .When(z => z.StopMonth.HasValue && z.StopYear.HasValue);

            RuleFor(r => r.StopDate)
               .Must((r, StopDate) => StopDate.IsEqualToOrAfterMonthYearOfDateTime(r.StartDate))
               .WithMessage(r => $"The stop date cannot be before the apprenticeship started")
               .When(r => r.StopDate.IsValid);

            RuleFor(r => r.StopDate)
                .Must((r, StopDate) => StopDate.IsNotInFutureMonthYear(_currentDateTime.UtcNow))
                .WithMessage(r => $"The stop date cannot be in the future")
                .When(r => r.StopDate.IsValid);

        }
    }
}

