using FluentValidation;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class DeleteDraftApprenticeshipViewModelValidator : AbstractValidator<DeleteDraftApprenticeshipViewModel>
{

    public DeleteDraftApprenticeshipViewModelValidator()
    {
        RuleFor(x => x.ConfirmDelete).NotNull().WithMessage("Confirm if you would like to delete this apprentice");
    }
}