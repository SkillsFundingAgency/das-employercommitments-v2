using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ChangeStatusRequestViewModelValidator : AbstractValidator<ChangeStatusRequestViewModel>
    {
        public ChangeStatusRequestViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.SelectedStatusChange).NotNull();
        }
    }
}
