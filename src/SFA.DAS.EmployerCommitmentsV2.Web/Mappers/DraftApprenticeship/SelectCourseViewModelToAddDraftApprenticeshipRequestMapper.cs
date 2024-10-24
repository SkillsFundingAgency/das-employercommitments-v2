﻿using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class SelectCourseViewModelToAddDraftApprenticeshipRequestMapper : IMapper<SelectCourseViewModel, AddDraftApprenticeshipRequest>
{
    public Task<AddDraftApprenticeshipRequest> Map(SelectCourseViewModel source)
    {
        return Task.FromResult(new AddDraftApprenticeshipRequest
        {
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            AccountHashedId = source.AccountHashedId,
            CohortId = source.CohortId,
            CohortReference = source.CohortReference,
            CourseCode = source.CourseCode,
            DeliveryModel = source.DeliveryModel,
            DraftApprenticeshipHashedId = source.DraftApprenticeshipHashedId,
            ProviderId = source.ProviderId,
            ReservationId = source.ReservationId ?? Guid.Empty,
            StartMonthYear = source.StartMonthYear,
            CacheKey = source.CacheKey
        });
    }
}