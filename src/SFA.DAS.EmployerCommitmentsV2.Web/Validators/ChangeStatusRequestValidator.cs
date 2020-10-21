using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
    {
        public ChangeStatusRequestValidator()
        {
            RuleFor(x => x.AccountHashedId).NotEmpty();
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
        }
    }
}
