using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ChangeVersionViewModelValidator : AbstractValidator<ChangeVersionViewModel>
{
    public ChangeVersionViewModelValidator()
    {
        RuleFor(r => r.SelectedVersion).NotNull().WithMessage("Select a version");
    }
}