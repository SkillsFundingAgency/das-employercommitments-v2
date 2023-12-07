using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ChangeOptionViewModelValidator : AbstractValidator<ChangeOptionViewModel>
{
    public ChangeOptionViewModelValidator()
    {
        RuleFor(r => r.SelectedOption).NotNull().WithMessage("Select an option");

        When(r => !string.IsNullOrEmpty(r.SelectedOption) && !r.ReturnToChangeVersion && !r.ReturnToEdit, () =>
        {
            RuleFor(r => r.SelectedOption).NotEqual(r => r.CurrentOption).WithMessage("Select a different option or cancel");
        });
    }
}