using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class PriceRequestValidator : AbstractValidator<EmployerLedChangeOfProviderRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(x => x.ProviderId).GreaterThan(0);
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.AccountHashedId).NotEmpty();
           /*TO DO : Include the validation for start and end date            
            RuleFor(x => x.NewStartMonth).Must(field => field.IsValidMonthYear());
            RuleFor(x => x.NewStartYear).Must(field => field.IsValidMonthYear()); */
        }
    }
}
