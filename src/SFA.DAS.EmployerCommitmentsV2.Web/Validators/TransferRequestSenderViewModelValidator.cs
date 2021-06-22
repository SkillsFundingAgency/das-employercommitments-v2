using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class TransferRequestSenderViewModelValidator : AbstractValidator<TransferRequestForSenderViewModel>
    {
        public TransferRequestSenderViewModelValidator()
        {
            RuleFor(x => x.ApprovalConfirmed)
                .NotNull()
                .WithMessage("Confirm if you want to accept or reject the transfer request");
        }
    }
}
