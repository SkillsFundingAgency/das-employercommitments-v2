﻿using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class AddDraftApprenticeshipViewModelValidator : AbstractValidator<AddDraftApprenticeshipViewModel>
{
    public AddDraftApprenticeshipViewModelValidator()
    {
        RuleFor(x => x.DateOfBirth).Must(y => y.IsValid).WithMessage("The Date of birth is not valid").When(z => z.DateOfBirth.HasValue);
        RuleFor(x => x.StartDate).Must(y => y.IsValid).WithMessage("The start date is not valid").When(z => z.StartDate.HasValue);
        RuleFor(x => x.EndDate).Must(y => y.IsValid).WithMessage("The end date is not valid").When(z => z.EndDate.HasValue);
    }
}