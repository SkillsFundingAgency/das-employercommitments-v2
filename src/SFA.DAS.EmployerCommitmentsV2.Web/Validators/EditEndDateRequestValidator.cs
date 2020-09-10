using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EditEndDateViewModelValidator : AbstractValidator<EditEndDateViewModel>
    {
        public EditEndDateViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.EndDate).Must(y => y.Month.HasValue || y.Year.HasValue).WithMessage("Enter the planned end date for this apprenticeship training");
            RuleFor(r => r.EndDate).Must(y => y.Year.HasValue).WithMessage("Planned training end date must include a year");
            RuleFor(r => r.EndDate).Must(y => y.Month.HasValue).WithMessage("Planned training end date must include a month");
            RuleFor(x => x.EndDate).Must(y => y.IsValid).WithMessage("Planned training end date must be a real date").When(z => z.EndDate.HasValue);
        }
    }
}
