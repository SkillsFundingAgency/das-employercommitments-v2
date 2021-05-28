using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ViewApprenticeshipUpdatesRequestViewModelValidator : AbstractValidator<ViewApprenticeshipUpdatesRequestViewModel>
    {
        public ViewApprenticeshipUpdatesRequestViewModelValidator()
        {
            RuleFor(r => r.UndoChanges).NotNull()
                .WithMessage("Confirm if you want to undo these changes");
        }
    }
}
