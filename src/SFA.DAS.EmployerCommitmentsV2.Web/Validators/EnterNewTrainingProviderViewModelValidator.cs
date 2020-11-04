using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class EnterNewTrainingProviderViewModelValidator : AbstractValidator<EnterNewTrainingProviderViewModel>
    {
        public EnterNewTrainingProviderViewModelValidator()
        {
            RuleFor(r => r.AccountHashedId).NotEmpty();
            RuleFor(r => r.ApprenticeshipHashedId).NotEmpty();
            RuleFor(r => r.Ukprn).GreaterThan(0).WithMessage("Select a training provider");
        }
    }
}
