using FluentValidation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Validators;

public class EditStopDateViewModelValidator : AbstractValidator<EditStopDateViewModel>
{
    private readonly ICurrentDateTime _currentDateTime;        

    public EditStopDateViewModelValidator(ICurrentDateTime currentDateTime)
    {
        _currentDateTime = currentDateTime;
            
        RuleFor(x => x.ApprenticeshipHashedId).NotEmpty();
            
        RuleFor(x => x.AccountHashedId).NotEmpty();

        RuleFor(r => r.NewStopDate)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Enter the stop date for this apprenticeship")
            .Must(d => d.Date.HasValue).WithMessage("Enter the stop date for this apprenticeship")
            .Must(d => d.Date <= new DateTime(_currentDateTime.UtcNow.Year, _currentDateTime.UtcNow.Month, 1)).WithMessage("The stop date cannot be in the future")
            .Must((model, newStopDate) => newStopDate.Date >= new DateTime(model.ApprenticeshipStartDate.Year, model.ApprenticeshipStartDate.Month, 1)).WithMessage("The stop month cannot be before the apprenticeship started")
            .Must((model, newStopDate) => newStopDate.Date != model.CurrentStopDate).WithMessage("Enter a date that is different to the current stopped date");            
    }       
}