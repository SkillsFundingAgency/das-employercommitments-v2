using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EditApprenticeshipRequestViewModelValidator : AbstractValidator<EditApprenticeshipRequestViewModel>
    {
        public EditApprenticeshipRequestViewModelValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty()
                .WithMessage("First name must be entered");

            RuleFor(r => r.LastName).NotEmpty()
               .WithMessage("Last name must be entered");

            RuleFor(r => r.FirstName).MaximumLength(100)
                .WithMessage("You must enter a first name that's no longer than 100 characters");

            RuleFor(r => r.LastName).MaximumLength(100)
               .WithMessage("You must enter a last name that's no longer than 100 characters");

            RuleFor(r => r.DateOfBirth).Must((r, DateOfBirth) => r.BirthMonth.HasValue && r.BirthYear.HasValue && r.BirthDay.HasValue)
                .WithMessage("The Date of birth is not valid");

            RuleFor(x => x.DateOfBirth)
                .Must(y => y.IsValid).WithMessage($"The Date of birth is not valid")
                .When(r => r.BirthMonth.HasValue && r.BirthYear.HasValue && r.BirthDay.HasValue);
        }
    }
}
