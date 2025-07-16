using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;

public class AddDraftApprenticeshipViewModelMapper : IMapper<AddDraftApprenticeshipRequest, AddDraftApprenticeshipViewModel>
{
    private readonly IApprovalsApiClient _client;
    private readonly IEncodingService _encodingService;

    public AddDraftApprenticeshipViewModelMapper(IApprovalsApiClient client, IEncodingService encodingService)
    {
        _client = client;
        _encodingService = encodingService;
    }

    public async Task<AddDraftApprenticeshipViewModel> Map(AddDraftApprenticeshipRequest source)
    {
        var accountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var startDate = new MonthYearModel(source.StartMonthYear).Date;
        var details = await _client.GetAddAnotherDraftApprenticeshipDetails(accountId, source.CohortId, source.CourseCode, startDate);

        if (details == null)
            throw new CohortEmployerUpdateDeniedException($"Cohort {source.CohortId} is not with the Employer");

        var result = new AddDraftApprenticeshipViewModel
        {
            AccountHashedId = source.AccountHashedId,
            CohortReference = source.CohortReference,
            CohortId = source.CohortId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            LegalEntityName = details.LegalEntityName,
            ReservationId = source.ReservationId,
            StartDate = new MonthYearModel(source.StartMonthYear),
            CourseCode = source.CourseCode,
            ProviderId = source.ProviderId,
            ProviderName = details.ProviderName,
            TransferSenderHashedId = details.IsFundedByTransfer ? _encodingService.Encode(details.TransferSenderId.Value, EncodingType.PublicAccountId) : string.Empty,
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false,
            CacheKey = source.CacheKey.IsNotNullOrEmpty() ? source.CacheKey : Guid.NewGuid(),
            StandardPageUrl = details.StandardPageUrl,
            FundingBandMax = details.ProposedMaxFunding
        };

        return result;
    }
}