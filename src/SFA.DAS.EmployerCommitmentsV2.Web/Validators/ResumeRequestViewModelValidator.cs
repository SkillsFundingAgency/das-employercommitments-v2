using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ResumeRequestViewModelValidator : AbstractValidator<ResumeRequestViewModel>
    {
        public ResumeRequestViewModelValidator()
        {
            RuleFor(r => r.ResumeConfirmed).NotNull()
                .WithMessage("Select whether to resume this apprenticeship or not");
        }
    }
}
