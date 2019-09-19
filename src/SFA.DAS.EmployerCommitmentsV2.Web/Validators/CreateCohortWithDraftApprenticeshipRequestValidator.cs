using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class CreateCohortWithDraftApprenticeshipRequestValidator : AbstractValidator<ApprenticeRequest>
    {
        public CreateCohortWithDraftApprenticeshipRequestValidator()
        {
            RuleFor(x => x.ProviderId).GreaterThan(0).WithMessage("Provider Id must be supplied");
            RuleFor(x => x.AccountLegalEntityId).GreaterThan(0).WithMessage("Account Legal Entity must be supplied");
            RuleFor(x => x.ReservationId).NotEmpty().WithMessage("Reservation ID must be supplied");
        }
    }
}
