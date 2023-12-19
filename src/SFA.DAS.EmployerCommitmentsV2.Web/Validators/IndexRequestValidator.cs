using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class IndexRequestValidator : AbstractValidator<IndexRequest>
{
    public IndexRequestValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.AccountLegalEntityHashedId).NotEmpty();
    }
}