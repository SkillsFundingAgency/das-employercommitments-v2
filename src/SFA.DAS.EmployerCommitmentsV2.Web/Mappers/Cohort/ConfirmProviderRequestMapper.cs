﻿using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ConfirmProviderRequestMapper : IMapper<SelectProviderViewModel, ConfirmProviderRequest>
{
    public Task<ConfirmProviderRequest> Map(SelectProviderViewModel source)
    {
        return Task.FromResult(new ConfirmProviderRequest
        {
            AccountHashedId = source.AccountHashedId,
            CourseCode = source.CourseCode,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = source.LegalEntityName,
            ProviderId = long.Parse(source.ProviderId),
            ReservationId = source.ReservationId,
            StartMonthYear = source.StartMonthYear,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            FundingType = source.FundingType
        });
    }
}