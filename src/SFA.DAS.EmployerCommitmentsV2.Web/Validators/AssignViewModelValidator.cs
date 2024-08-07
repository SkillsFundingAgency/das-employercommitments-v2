﻿using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class AssignViewModelValidator : AbstractValidator<AssignViewModel>
{
    public AssignViewModelValidator()
    {
        When(r =>
        r.WhoIsAddingApprentices.HasValue
        && r.WhoIsAddingApprentices.Value == WhoIsAddingApprentices.Provider
        && !string.IsNullOrEmpty(r.Message), () =>
        {
            RuleFor(x => x.Message).ValidFreeTextCharacters().WithErrorCode("Text");
        });
    }
}