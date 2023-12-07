

using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ViewChangesRequestValidator : AbstractValidator<ViewChangesRequest>
{
    public ViewChangesRequestValidator()
    {
        RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.ApprenticeshipId).GreaterThan(0);
    }
}