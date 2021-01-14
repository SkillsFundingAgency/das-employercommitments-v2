using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EnterNewTrainingProviderViewModelValidator : AbstractValidator<EnterNewTrainingProviderViewModel>
    {
        public EnterNewTrainingProviderViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.ProviderId).GreaterThan(0).WithMessage("Select a training provider");
            RuleFor(r => r.ProviderId).NotEqual(r => r.CurrentProviderId).WithMessage("Select another training provider - you cannot select the current training provider as the new training provider ");
        }
    }
}
