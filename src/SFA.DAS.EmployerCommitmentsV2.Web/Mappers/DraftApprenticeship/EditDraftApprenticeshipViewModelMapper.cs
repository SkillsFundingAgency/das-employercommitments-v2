﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class EditDraftApprenticeshipViewModelMapper : IMapper<EditDraftApprenticeshipRequest, IDraftApprenticeshipViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IEncodingService _encodingService;
    private readonly IApprovalsApiClient _apiClient;

    public EditDraftApprenticeshipViewModelMapper(IEncodingService encodingService, IApprovalsApiClient apiClient)
    {
        _encodingService = encodingService;
        _apiClient = apiClient;
    }

    public async Task<IDraftApprenticeshipViewModel> Map(EditDraftApprenticeshipRequest source)
    {
        var cohort = source.Cohort;

        var draftApprenticeship = await _apiClient.GetEditDraftApprenticeship(source.Request.AccountId, source.Request.CohortId,
            source.Request.DraftApprenticeshipId);

        return new EditDraftApprenticeshipViewModel(draftApprenticeship.DateOfBirth, draftApprenticeship.StartDate, draftApprenticeship.EndDate)
        {
            DraftApprenticeshipId = source.Request.DraftApprenticeshipId,
            DraftApprenticeshipHashedId = _encodingService.Encode(source.Request.DraftApprenticeshipId, EncodingType.ApprenticeshipId),
            CohortId = source.Request.CohortId,
            CohortReference = _encodingService.Encode(source.Request.CohortId, EncodingType.CohortReference),
            ReservationId = draftApprenticeship.ReservationId,
            FirstName = draftApprenticeship.FirstName,
            LastName = draftApprenticeship.LastName,
            Email = draftApprenticeship.Email,
            Uln = draftApprenticeship.Uln,
            DeliveryModel = (DeliveryModel)draftApprenticeship.DeliveryModel,
            CourseCode = draftApprenticeship.CourseCode,
            StandardUId = draftApprenticeship.StandardUId,
            Cost = draftApprenticeship.Cost,
            EmploymentPrice = draftApprenticeship.EmploymentPrice,
            EmploymentEndMonth = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Month : (int?)null,
            EmploymentEndYear = draftApprenticeship.EmploymentEndDate.HasValue ? draftApprenticeship.EmploymentEndDate.Value.Year : (int?)null,
            Reference = draftApprenticeship.EmployerReference,
            AccountHashedId = source.Request.AccountHashedId,
            ProviderId = source.Cohort.ProviderId.Value,
            ProviderName = cohort.ProviderName,
            LegalEntityName = source.Cohort.LegalEntityName,
            IsContinuation = draftApprenticeship.IsContinuation,
            AccountLegalEntityId = cohort.AccountLegalEntityId,
            AccountLegalEntityHashedId = _encodingService.Encode(cohort.AccountLegalEntityId, EncodingType.PublicAccountLegalEntityId),
            HasMultipleDeliveryModelOptions = draftApprenticeship.HasMultipleDeliveryModelOptions,
            HasUnavailableFlexiJobDeliveryModel = draftApprenticeship.HasUnavailableDeliveryModel && draftApprenticeship.DeliveryModel == EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel.FlexiJobAgency,
            IsOnFlexiPaymentPilot = draftApprenticeship.IsOnFlexiPaymentPilot ?? false,
            ActualStartDate = draftApprenticeship.ActualStartDate,
            ActualEndDate = (draftApprenticeship.IsOnFlexiPaymentPilot ?? false) ? draftApprenticeship.EndDate : null,
            CourseName = draftApprenticeship.CourseName,
            EmailAddressConfirmed = draftApprenticeship.EmailAddressConfirmed,
            RecognisePriorLearning = draftApprenticeship.RecognisePriorLearning,
            TrainingTotalHours = draftApprenticeship.TrainingTotalHours,
            DurationReducedByHours = draftApprenticeship.DurationReducedByHours,
            DurationReducedBy = draftApprenticeship.DurationReducedBy,
            PriceReducedBy = draftApprenticeship.PriceReducedBy,
            CacheKey = source.Request.CacheKey.IsNotNullOrEmpty() ? source.Request.CacheKey : Guid.NewGuid(),
            StandardPageUrl = draftApprenticeship.StandardPageUrl,
            FundingBandMax = draftApprenticeship.ProposedMaxFunding
        };
    }
}