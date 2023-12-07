using System.Globalization;
using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class AddDraftApprenticeshipRequestValidator : AbstractValidator<AddDraftApprenticeshipRequest>
{
    public AddDraftApprenticeshipRequestValidator()
    {
        RuleFor(r => r.AccountHashedId).NotEmpty();
        RuleFor(r => r.CohortReference).NotEmpty();
        RuleFor(r => r.CohortId).GreaterThanOrEqualTo(1);
        RuleFor(r => r.AccountLegalEntityHashedId).NotEmpty();
        RuleFor(r => r.AccountLegalEntityId).GreaterThanOrEqualTo(1);
        RuleFor(r => r.ReservationId).NotEmpty();
            
        RuleFor(r => r.StartMonthYear)
            .Must(s => DateTime.TryParseExact(s, "MMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            .When(r => !string.IsNullOrWhiteSpace(r.StartMonthYear));
    }
}