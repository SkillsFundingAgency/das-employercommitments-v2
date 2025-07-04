using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditApprenticeshipViewModelToValidateEditApprenticeshipRequestMapper : IMapper<EditApprenticeshipRequestViewModel, ValidateEditApprenticeshipRequest>
{
    public Task<ValidateEditApprenticeshipRequest> Map(EditApprenticeshipRequestViewModel source)
    {
        var result = new ValidateEditApprenticeshipRequest
        {
            EmployerAccountId = source.AccountId,
            ApprenticeshipId = source.ApprenticeshipId,
            ProviderId = source.ProviderId,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email,
            DateOfBirth = source.DateOfBirth.Date,
            ULN = source.ULN,
            Cost = source.Cost,
            EmployerReference = source.EmployerReference,
            StartDate = source.StartDate.Date,
            EndDate = source.EndDate.Date,
            DeliveryModel = (int)source.DeliveryModel,
            CourseCode = source.CourseCode,
            Version = source.Version,
            Option = source.Option == "TBC" ? string.Empty : source.Option,
            EmploymentEndDate = source.EmploymentEndDate.Date,
            EmploymentPrice = source.EmploymentPrice,
        };
        
        return Task.FromResult(result);
    }
}