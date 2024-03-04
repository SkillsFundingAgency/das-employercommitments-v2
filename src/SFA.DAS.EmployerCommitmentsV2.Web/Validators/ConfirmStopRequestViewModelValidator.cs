using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmStopRequestViewModelValidator : AbstractValidator<ConfirmStopRequestViewModel>
{
    public ConfirmStopRequestViewModelValidator()
    {
        RuleFor(r => r.StopConfirmed).NotNull().WithMessage("Select whether to stop this apprenticeship or not");
    }
}