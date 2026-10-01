using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ApprenticeshipApprovalRequestAlertsViewModelValidator : AbstractValidator<ApprenticeshipApprovalRequestAlertsViewModel>
{
    public ApprenticeshipApprovalRequestAlertsViewModelValidator()
    {
        RuleFor(x => x.ApprovalRequests)
          .Must(r => r != null && r.All(x => x.Seen.HasValue)).WithMessage("Select if you would like to delete this alert");

        RuleForEach(x => x.ApprovalRequests)
            .ChildRules(r =>
            {
                r.RuleFor(k => k.Seen).NotNull()
                .WithMessage("Select if you would like to delete this alert");
            });
    }
}