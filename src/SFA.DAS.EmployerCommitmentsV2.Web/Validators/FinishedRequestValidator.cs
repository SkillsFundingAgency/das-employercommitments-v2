using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class FinishedRequestValidator : AbstractValidator<FinishedRequest>
{
    public FinishedRequestValidator()
    {
        RuleFor(r => r.AccountHashedId).NotEmpty();
        RuleFor(r => r.AccountId).GreaterThan(0);
        RuleFor(r => r.CohortReference).NotEmpty();
        RuleFor(r => r.CohortId).GreaterThan(0);
    }
}