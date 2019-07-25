using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ConfirmProviderViewModelValidator : AbstractValidator<ConfirmProviderViewModel>
    {
        public ConfirmProviderViewModelValidator()
        {
            RuleFor(x => x.UseThisProvider).Must(x => x.HasValue).WithMessage("Select a training provider");
        }
    }
}