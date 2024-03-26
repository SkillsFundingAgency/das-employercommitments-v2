using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class DetailsViewModelValidator : AbstractValidator<DetailsViewModel>
{
    public DetailsViewModelValidator()
    {
        RuleFor(x => x.Selection).NotEmpty().WithMessage("You must choose an option");
    }
}