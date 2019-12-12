using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class CohortsByAccountRequestValidator : AbstractValidator<CohortsByAccountRequest>
    {
        public CohortsByAccountRequestValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.AccountHashedId).NotEmpty();
        }
    }
}
