using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EditApprenticeshipRequestValidator : AbstractValidator<EditApprenticeshipRequest>
    {
        public EditApprenticeshipRequestValidator()
        {
            RuleFor(x => x.CacheKey).NotEmpty();
        }
    }
}