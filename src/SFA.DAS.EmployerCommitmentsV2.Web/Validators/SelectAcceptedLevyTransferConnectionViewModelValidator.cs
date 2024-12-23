using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SelectAcceptedLevyTransferConnectionViewModelValidator : AbstractValidator<SelectAcceptedLevyTransferConnectionViewModel>
{
    public SelectAcceptedLevyTransferConnectionViewModelValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.ApplicationAndSenderHashedId).NotEmpty().WithMessage("Select a transfer");            
    }
}