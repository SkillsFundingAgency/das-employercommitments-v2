using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Data;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhyStopApprenticeshipViewModelValidator:AbstractValidator<WhyStopApprenticeshipViewModel>
    {
        public WhyStopApprenticeshipViewModelValidator()
        {
            RuleFor(x=>x.AccountHashedId).NotEmpty();
            RuleFor(x=>x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.SelectedStatusChange).NotNull().WithMessage("You need to select why you want to stop this apprenticeship");
        }
    }
}
