﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class DeleteDraftApprenticeshipRequestValidator : AbstractValidator<DeleteApprenticeshipRequest>
{
    public DeleteDraftApprenticeshipRequestValidator()
    {
        RuleFor(r => r.AccountHashedId).NotEmpty();
        RuleFor(r => r.CohortReference).NotEmpty();
        RuleFor(r => r.CohortId).GreaterThanOrEqualTo(1);
        RuleFor(r => r.DraftApprenticeshipId).GreaterThanOrEqualTo(1);
    }
}