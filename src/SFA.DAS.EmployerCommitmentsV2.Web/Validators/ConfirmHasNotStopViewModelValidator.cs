using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmHasNotStopViewModelValidator : AbstractValidator<ConfirmHasNotStopViewModel>
{
    public ConfirmHasNotStopViewModelValidator()
    {
        RuleFor(r => r.StopConfirmed).NotNull().WithMessage("You need to confirm if the apprenticeship has stopped");
    }
}