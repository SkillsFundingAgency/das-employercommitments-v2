﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class AddDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;

    public AddDraftApprenticeshipViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _encodingService = encodingService;
    }
        
    public async Task<AddDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
    {
        var cohort = await _commitmentsApiClient.GetCohort(source.CohortId);

        if (cohort.WithParty != Party.Employer)
            throw new CohortEmployerUpdateDeniedException($"Cohort {cohort} is not with the Employer");

        var result = new AddDraftApprenticeshipViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CohortReference = source.CohortReference,
            CohortId = source.CohortId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            LegalEntityName = cohort.LegalEntityName,
            ReservationId = source.ReservationId,
            StartDate = new MonthYearModel(source.StartMonthYear),
            CourseCode = source.CourseCode,
            ProviderId = source.ProviderId,
            ProviderName = cohort.ProviderName,
            Courses = null,
            TransferSenderHashedId = cohort.IsFundedByTransfer ? _encodingService.Encode(cohort.TransferSenderId.Value, EncodingType.PublicAccountId) : string.Empty,
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false,
            CacheKey = source.CacheKey.IsNotNullOrEmpty() ? source.CacheKey : Guid.NewGuid()
        };

        return result;
    }
}