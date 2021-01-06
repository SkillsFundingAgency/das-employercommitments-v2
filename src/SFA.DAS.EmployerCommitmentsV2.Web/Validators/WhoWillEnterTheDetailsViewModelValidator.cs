using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhoWillEnterTheDetailsViewModelValidator : AbstractValidator<WhoWillEnterTheDetailsViewModel>
    {
        public WhoWillEnterTheDetailsViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.ProviderId).NotEmpty();
            RuleFor(r => r.EmployerResponsibility).NotNull().WithMessage("Select who will enter the new course dates and price");
        }
    }
}
