using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ApproveRequestValidator : AbstractValidator<ApproveRequest>
    {
        public ApproveRequestValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.CohortReference).NotEmpty();
        }
    }
}