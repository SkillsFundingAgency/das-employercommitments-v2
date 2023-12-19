using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SendNewTrainingProviderRequestValidator : AbstractValidator<SendNewTrainingProviderRequest>
{
    public SendNewTrainingProviderRequestValidator()
    {
        RuleFor(r => r.AccountHashedId).NotEmpty();
        RuleFor(r => r.AccountId).GreaterThanOrEqualTo(1);
        RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
        RuleFor(r => r.ApprenticeshipId).GreaterThanOrEqualTo(1);
        RuleFor(r => r.ProviderId).GreaterThanOrEqualTo(1);
    }
}