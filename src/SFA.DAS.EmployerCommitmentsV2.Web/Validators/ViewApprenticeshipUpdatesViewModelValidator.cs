using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ViewApprenticeshipUpdatesViewModelValidator : AbstractValidator<ViewApprenticeshipUpdatesViewModel>
    {
        public ViewApprenticeshipUpdatesViewModelValidator()
        {
            RuleFor(r => r.UndoChanges).NotNull()
                .WithMessage("Confirm if you want to undo these changes");
        }
    }
}
