using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmailValidationService;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class DraftApprenticeshipViewModelValidator : AbstractValidator<DraftApprenticeshipViewModel>
{
    private const string EmailAddressEmptyErrorMessage = "Enter an email address";
    private const string EmailAddressFormatErrorMessage = "Enter an email address in the correct format";

    public DraftApprenticeshipViewModelValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name must not be empty");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name must not be empty");

        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(EmailAddressEmptyErrorMessage)
            .NotEmpty().WithMessage(EmailAddressEmptyErrorMessage);

        RuleFor(x => x.Email)
            .Must((model, _) => model.Email.IsAValidEmailAddress())
            .WithMessage(EmailAddressFormatErrorMessage)
            .When(model => !string.IsNullOrWhiteSpace(model.Email));

        RuleFor(x => x.DateOfBirth).Must(y => y.IsValid).WithMessage("The Date of birth is not valid").When(z => z.DateOfBirth.HasValue);
        RuleFor(x => x.StartDate).Must(y => y.IsValid).WithMessage("The start date is not valid").When(z => z.StartDate.HasValue);
        RuleFor(x => x.EndDate).Must(y => y.IsValid).WithMessage("The end date is not valid").When(z => z.EndDate.HasValue);
        RuleFor(x => x.DeliveryModel).NotNull().WithMessage("Select a delivery model");
    }
}