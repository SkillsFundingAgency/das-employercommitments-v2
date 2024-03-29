﻿using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ConfirmStopRequestVMToStopApprenticeshpRequestMapper : IMapper<ConfirmStopRequestViewModel, StopApprenticeshipRequest>
{
    public Task<StopApprenticeshipRequest> Map(ConfirmStopRequestViewModel source)
    {
        return Task.FromResult(new StopApprenticeshipRequest
        {
            AccountId = source.AccountId,
            StopDate = source.StopDate,
            MadeRedundant = source.MadeRedundant.Value
        });
    }
}