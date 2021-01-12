using FluentValidation;
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
            RuleFor(x => x.NewPrice).NotEmpty().WithMessage("Enter the agreed price of completing the training with the new provider");
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(1).WithMessage("The agreed price of completing the training with the new provider must be more than 0");
            RuleFor(x => x.NewPrice).LessThanOrEqualTo(100000).WithMessage("The agreed price of completing the training with the new provider must be less than 100000");           
        }
    }
}
