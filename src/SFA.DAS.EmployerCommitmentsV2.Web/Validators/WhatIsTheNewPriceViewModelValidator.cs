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
            RuleFor(x => x.NewStartDate).Must(field => field.IsValidMonthYear());
            RuleFor(x => x.NewEndDate).Must(field => field.IsValidMonthYear());
            RuleFor(x => x.NewPrice).NotEmpty().WithMessage("Enter the agreed price of completing the training with the new provider");
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(1).WithMessage("Enter the new agreed apprenticeship price");
            RuleFor(x => x.NewPrice).LessThanOrEqualTo(100000).WithMessage("The agreed price of completing the training with the new provider must be less than 100000");
            // TO DO : AC2 and AC 4
            //RuleFor(x => x.NewPrice).LessThanOrEqualTo(0).WithMessage("The agreed price of completing the training with the new provider must be more than 0"); // AC4
            //Validate end date: Entered value is not a number → error THEN I am presented with the following error summary and message: The agreed price of completing the training with the new provider must be a number //AC2
        }
    }
}
