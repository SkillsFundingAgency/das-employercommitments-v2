using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ReviewApprenticeshipUpdatesViewModelValidator : AbstractValidator<ReviewApprenticeshipUpdatesViewModel>
    {
        public ReviewApprenticeshipUpdatesViewModelValidator()
        {
            RuleFor(r => r.ApproveChanges).NotNull()
                .WithMessage("Confirm if you are happy to approve these changes");
        }
    }
}
