using System;
using System.Globalization;
using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators
{
    public class SelectDeliveryModelViewModelValidator : AbstractValidator<SelectDeliveryModelViewModel>
    {
        public SelectDeliveryModelViewModelValidator()
        {
            RuleFor(x => x.DeliveryModel).NotNull().WithMessage("You must select the apprenticeship delivery model");
        }
    }
}