using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class StopRequestValidator : AbstractValidator<StopRequest>
{
    public StopRequestValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
    }
}