using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ConfirmHasValidEndDateViewModelValidator : AbstractValidator<ConfirmHasValidEndDateViewModel>
    {
        public ConfirmHasValidEndDateViewModelValidator()
        {
            RuleFor(r => r.EndDateConfirmed).NotNull().WithMessage("You need to confirm if the planned end date is correct");
        }
    }
}