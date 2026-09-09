using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ApprenticeshipApprovalRequestViewModelValidator : AbstractValidator<ApprenticeshipApprovalRequestViewModel>
{
    public ApprenticeshipApprovalRequestViewModelValidator()
    {
        RuleFor(r => r.ApproveChanges)
            .NotNull()
            .WithMessage("Select if you want to approve these changes");
    }
}