using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class DataLockRequestChangesViewModelValidator : AbstractValidator<DataLockRequestChangesViewModel>
    {
        public DataLockRequestChangesViewModelValidator()
        {
            RuleFor(r => r.AcceptChanges).NotNull()
                .WithMessage("Confirm if you are happy to approve these changes");
        }
    }
}
