using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class SelectLegalEntityViewModelValidator : AbstractValidator<SelectLegalEntityViewModel>
    {
        public SelectLegalEntityViewModelValidator()
        {            
            RuleFor(x => x.LegalEntityCode).NotEmpty().WithMessage("Choose organisation");
        }
    }
}
