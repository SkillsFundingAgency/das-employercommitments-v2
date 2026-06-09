using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ChangePaymentsRequestViewModelValidator : AbstractValidator<ChangePaymentsRequestViewModel>
{
    public ChangePaymentsRequestViewModelValidator()
    {
        RuleFor(r => r.ChangeConfirmed)
            .NotNull()
            .WithMessage(r => r.FreezeStatus
                ? "Select if you would like to resume payments"
                : "Select if you would like to pause payments");

        RuleFor(r => r.FreezePaymentsReason)
            .Must(r => r is >= 1 and <= 4)
            .When(r => !r.FreezeStatus && r.ChangeConfirmed != false)
            .WithMessage("Select a reason for pausing payments");
    }
}
