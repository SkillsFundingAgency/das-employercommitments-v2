using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmWhenApprenticeshipStoppedViewModelValidator : AbstractValidator<ConfirmWhenApprenticeshipStoppedViewModel>
{
    public ConfirmWhenApprenticeshipStoppedViewModelValidator()
    {
        RuleFor(r => r.IsCorrectStopDate).NotNull().WithMessage("You need to confirm if this stop date is correct");
    }
}