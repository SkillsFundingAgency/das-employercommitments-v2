using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ConfirmEditApprenticeshipRequestValidator : AbstractValidator<ConfirmEditApprenticeshipRequest>
    {
        public ConfirmEditApprenticeshipRequestValidator()
        {
            RuleFor(x => x.CacheKey).NotEmpty();
        }
    }
}