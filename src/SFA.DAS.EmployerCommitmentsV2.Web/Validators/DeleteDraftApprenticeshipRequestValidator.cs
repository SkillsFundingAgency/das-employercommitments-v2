using System;
using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Enums;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class DeleteDraftApprenticeshipRequestValidator : AbstractValidator<DeleteApprenticeshipRequest>
    {
        public DeleteDraftApprenticeshipRequestValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.CohortReference).NotEmpty();
            RuleFor(r => r.CohortId).GreaterThanOrEqualTo(1);
            RuleFor(r => r.DraftApprenticeshipId).GreaterThanOrEqualTo(1);

            //RuleFor(r => r.Origin)
            //    .Must(s => !string.IsNullOrWhiteSpace(s))
            //    .Must(s => Enum.TryParse(s, out Origin _));


        }
    }
}
