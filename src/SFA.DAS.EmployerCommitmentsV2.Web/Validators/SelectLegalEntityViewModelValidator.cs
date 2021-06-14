using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class SelectLegalEntityViewModelValidator : AbstractValidator<SelectLegalEntityViewModel>
    {
        public SelectLegalEntityViewModelValidator()
        {            
            RuleFor(x => x.LegalEntityId).NotEmpty().WithMessage("Choose organisation");
        }
    }
}
