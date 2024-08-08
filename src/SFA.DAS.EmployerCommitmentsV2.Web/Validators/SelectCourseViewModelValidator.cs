using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class SelectCourseViewModelValidator : AbstractValidator<SelectCourseViewModel>
    {
        public SelectCourseViewModelValidator()
        {
            RuleFor(x => x.CourseCode)
                .NotEmpty()
                .WithMessage("Select an apprenticeship course");
        }
    }
}
