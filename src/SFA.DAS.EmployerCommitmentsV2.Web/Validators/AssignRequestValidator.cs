using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class AssignRequestValidator : AbstractValidator<AssignRequest>
{
    public AssignRequestValidator()
    {
        RuleFor(x => x.ProviderId).GreaterThan(0);
    }
}