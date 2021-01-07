using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhoWillEnterTheDetailsViewModelValidator : AbstractValidator<WhoWillEnterTheDetailsViewModel>
    {
        public WhoWillEnterTheDetailsViewModelValidator()
        {
            RuleFor(r => r.EmployerResponsibility).NotNull().WithMessage("Select who will enter the new course dates and price");
        }
    }
}
