using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ReviewApprenticeshipUpdatesRequestViewModelValidator : AbstractValidator<ReviewApprenticeshipUpdatesRequestViewModel>
    {
        public ReviewApprenticeshipUpdatesRequestViewModelValidator()
        {
            RuleFor(r => r.ApproveChanges).NotNull()
                .WithMessage("Confirm if you are happy to approve these changes");
        }
    }
}
