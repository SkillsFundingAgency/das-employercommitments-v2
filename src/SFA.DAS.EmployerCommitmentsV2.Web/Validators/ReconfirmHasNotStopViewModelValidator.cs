using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ReconfirmHasNotStopViewModelValidator : AbstractValidator<ReconfirmHasNotStopViewModel>
{
    public ReconfirmHasNotStopViewModelValidator()
    {
        RuleFor(r => r.StopConfirmed).NotNull().WithMessage("You need to confirm the apprenticeship has not stopped");
    }
}