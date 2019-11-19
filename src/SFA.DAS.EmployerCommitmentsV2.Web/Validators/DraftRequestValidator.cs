using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class DraftRequestValidator : AbstractValidator<DraftRequest>
    {
        public DraftRequestValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.AccountHashedId).NotEmpty();
        }
    }
}
