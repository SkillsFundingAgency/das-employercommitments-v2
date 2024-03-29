﻿using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;


namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class EditApprenticeshipViewModelToValidateApprenticeshipForEditMapper : IMapper<EditApprenticeshipRequestViewModel, ValidateApprenticeshipForEditRequest>
{
    public Task<ValidateApprenticeshipForEditRequest> Map(EditApprenticeshipRequestViewModel source)
    {
        var result = new ValidateApprenticeshipForEditRequest
        {
            EmployerAccountId = source.AccountId,
            ApprenticeshipId = source.ApprenticeshipId,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email,
            DateOfBirth = source.DateOfBirth.Date,
            ULN = source.ULN,
            Cost = source.Cost,
            EmployerReference = source.EmployerReference,
            StartDate = source.StartDate.Date,
            EndDate = source.EndDate.Date,
            DeliveryModel = source.DeliveryModel,
            TrainingCode = source.CourseCode,
            Version = source.Version,
            Option = source.Option == "TBC" ? string.Empty : source.Option,
            EmploymentEndDate = source.EmploymentEndDate.Date,
            EmploymentPrice = source.EmploymentPrice,
        };
        
        return Task.FromResult(result);
    }
}