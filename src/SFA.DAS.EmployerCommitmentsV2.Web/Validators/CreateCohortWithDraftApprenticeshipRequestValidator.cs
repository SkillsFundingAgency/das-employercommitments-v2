using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class CreateCohortWithDraftApprenticeshipRequestValidator : AbstractValidator<ApprenticeRequest>
{
    public CreateCohortWithDraftApprenticeshipRequestValidator()
    {
        RuleFor(x => x.AccountLegalEntityId).GreaterThan(0).WithMessage("Account Legal Entity must be supplied");
    }
}