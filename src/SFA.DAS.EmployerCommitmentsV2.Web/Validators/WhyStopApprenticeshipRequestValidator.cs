using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class WhyStopApprenticeshipRequestValidator:AbstractValidator<WhyStopApprenticeshipRequest>
{
    public WhyStopApprenticeshipRequestValidator()
    {
        RuleFor(x=>x.AccountHashedId).NotEmpty();
        RuleFor(x=>x.ApprenticeshipHashedId).NotEmpty();
    }
}