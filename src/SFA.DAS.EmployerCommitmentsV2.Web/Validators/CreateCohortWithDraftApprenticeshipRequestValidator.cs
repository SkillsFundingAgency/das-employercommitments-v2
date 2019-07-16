using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class CreateCohortWithDraftApprenticeshipRequestValidator : AbstractValidator<CreateCohortWithDraftApprenticeshipRequest>
    {
        public CreateCohortWithDraftApprenticeshipRequestValidator()
        {
            RuleFor(x => x.ProviderId).GreaterThan(0).WithMessage("Provider Id must be supplied");
            RuleFor(x => x.AccountLegalEntityId).GreaterThan(0).WithMessage("Account Legal Entity must be supplied");
        }
    }
}
