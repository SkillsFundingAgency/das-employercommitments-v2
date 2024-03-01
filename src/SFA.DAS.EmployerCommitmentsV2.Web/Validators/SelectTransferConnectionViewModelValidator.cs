using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class SelectTransferConnectionViewModelValidator : AbstractValidator<SelectTransferConnectionViewModel>
{
    public SelectTransferConnectionViewModelValidator()
    {
        RuleFor(x => x.AccountHashedId).NotEmpty();
        RuleFor(x => x.TransferConnectionCode).NotEmpty().WithMessage("Please choose an option");            
    }
}