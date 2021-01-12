using FluentValidation;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class WhatIsTheNewPriceViewModelValidator : AbstractValidator<WhatIsTheNewPriceViewModel>
    {
        public WhatIsTheNewPriceViewModelValidator()    
        {
            RuleFor(x => x.ProviderId).GreaterThan(0);      
            RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            RuleFor(x => x.AccountHashedId).NotEmpty();            
            RuleFor(x => x.NewPrice).NotEmpty()
                .WithMessage("Enter the agreed price of completing the training with the new provider");
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(1)
                .WithMessage("The agreed price of completing the training with the new provider must be more than 0");
            RuleFor(x => x.NewPrice).LessThanOrEqualTo(100000)
                .WithMessage("The agreed price of completing the training with the new provider must be less than 100000");
            RuleFor(x => x.NewStartMonthYear).Must(field => field.IsValidMonthYear());                
            RuleFor(x => x.NewEndMonthYear).Must(field => field.IsValidMonthYear());
            //RuleFor(x => x.NewEndMonthYear.IsValidMonthYear()).GreaterThan(y => y.NewStartMonthYear.IsValidMonthYear())
            //    .WithMessage("New End Month Year must be greater than New Start Month Year");

            //RuleFor(x => x.NewStartMonthYear.IsValidMonthYear()).GreaterThan(y => y.NewEndMonthYear.IsValidMonthYear())
            //    .WithMessage("New End Month Year must be greater than New Start Month Year");          
                           
        }
    }
}
