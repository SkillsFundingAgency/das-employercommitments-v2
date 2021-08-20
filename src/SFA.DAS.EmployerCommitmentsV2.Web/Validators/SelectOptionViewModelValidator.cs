using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class SelectOptionViewModelValidator : AbstractValidator<SelectOptionViewModel>
    {
        public SelectOptionViewModelValidator()
        {
            RuleFor(vm => vm.CourseOption).NotNull().WithMessage("Select an option");
        }
    }
}
