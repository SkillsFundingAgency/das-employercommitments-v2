using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SelectFundingViewModelValidator : AbstractValidator<SelectFundingViewModel>
{
    public SelectFundingViewModelValidator()
    {            
        RuleFor(x => x.FundingType).NotEmpty().WithMessage("Select funding type");
    }
}