using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class ConfirmStopRequestValidator : AbstractValidator<ConfirmStopRequest>
{
    public ConfirmStopRequestValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
        RuleFor(x => x.MadeRedundant).NotNull();
    }
}