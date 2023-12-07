using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class CancelChangeOfProviderRequestViewModelValidator : AbstractValidator<CancelChangeOfProviderRequestViewModel>
{
    public CancelChangeOfProviderRequestViewModelValidator()
    {
        RuleFor(r => r.CancelRequest).NotNull().WithMessage("Select if you want to cancel this request");
    }
}