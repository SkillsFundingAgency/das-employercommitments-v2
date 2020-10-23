using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class PauseRequestViewModelValidator : AbstractValidator<PauseRequestViewModel>
    {
        public PauseRequestViewModelValidator()
        {
            RuleFor(r => r.PauseConfirmed).NotNull().WithMessage("Select whether to pause this apprenticeship or not");
        }
    }
}
