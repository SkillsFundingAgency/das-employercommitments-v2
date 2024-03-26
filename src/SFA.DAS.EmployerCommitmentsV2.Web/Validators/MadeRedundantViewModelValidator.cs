using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class MadeRedundantViewModelValidator : AbstractValidator<MadeRedundantViewModel>
{
    public MadeRedundantViewModelValidator()
    {
        RuleFor(r => r.MadeRedundant).NotNull().WithMessage("Select yes if the apprentice has been made redundant");
    }
}