using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class TransferConfirmationViewModelValidator : AbstractValidator<TransferConfirmationViewModel>
    {
        public TransferConfirmationViewModelValidator()
        {
            RuleFor(x => x.SelectedOption)
                .NotNull()
                .WithMessage("Confirm what you want to do next");
        }
    }
}
