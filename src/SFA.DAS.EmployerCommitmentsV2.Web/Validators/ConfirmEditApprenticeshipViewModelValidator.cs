using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmEditApprenticeshipViewModelValidator : AbstractValidator<ConfirmEditApprenticeshipViewModel>
{
    public ConfirmEditApprenticeshipViewModelValidator()
    {
        RuleFor(r => r.ConfirmChanges).NotNull()
            .WithMessage("Select an option");
    }
}