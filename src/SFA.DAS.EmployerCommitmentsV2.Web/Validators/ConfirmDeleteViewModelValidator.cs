using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ConfirmDeleteViewModelValidator : AbstractValidator<ConfirmDeleteViewModel>
    {
        public ConfirmDeleteViewModelValidator()
        {
            RuleFor(x => x.ConfirmDeletion).NotNull().WithMessage("You must choose an option");
        }
    }
}
