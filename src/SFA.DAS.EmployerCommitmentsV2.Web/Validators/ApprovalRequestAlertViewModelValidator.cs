using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ApprovalRequestAlertViewModelValidator: AbstractValidator<ApprovalRequestAlertViewModel>
{
    public ApprovalRequestAlertViewModelValidator()
    {
        RuleFor(x => x.EmployerAcknowlededAt).NotNull().WithMessage("Select if you would like to delete this alert");
    }
}
