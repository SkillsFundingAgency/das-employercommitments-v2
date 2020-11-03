using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ChangeProviderRequestedConfirmationValidator : AbstractValidator<ChangeProviderRequestedConfirmationRequest>
    {
        public ChangeProviderRequestedConfirmationValidator()
        {
            RuleFor(x => x.AccountHashedId).NotEmpty();
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.ProviderId).GreaterThan(0);
        }
    }
}
