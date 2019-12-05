using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WithTransferSenderRequestValidator : AbstractValidator<WithTransferSenderRequest>
    {
        public WithTransferSenderRequestValidator()
        {
            RuleFor(x => x.AccountId).GreaterThan(0);
            RuleFor(x => x.AccountHashedId).NotEmpty();
        }
    }
}
