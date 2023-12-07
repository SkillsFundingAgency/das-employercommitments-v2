using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class ApprenticeViewModelMapper : IMapper<ApprenticeRequest, ApprenticeViewModel>
{
    private readonly ICommitmentsApiClient _commitmentsApiClient;
    private readonly IAuthorizationService _authorizationService;

    public ApprenticeViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IAuthorizationService authorizationService)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _authorizationService = authorizationService;
    }

    public async Task<ApprenticeViewModel> Map(ApprenticeRequest source)
    {
        var ale = await _commitmentsApiClient.GetAccountLegalEntity(source.AccountLegalEntityId);

        var provider = await _commitmentsApiClient.GetProvider(source.ProviderId);

        var result = new ApprenticeViewModel
        {
            AccountHashedId = source.AccountHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AccountLegalEntityHashedId = source.AccountLegalEntityHashedId,
            LegalEntityName = ale.LegalEntityName,
            StartDate = new MonthYearModel(source.StartMonthYear),
            ReservationId = source.ReservationId,
            CourseCode = source.CourseCode,
            ProviderId = (int)source.ProviderId,
            ProviderName = provider.Name,
            Courses = null,
            TransferSenderId = source.TransferSenderId,
            EncodedPledgeApplicationId = source.EncodedPledgeApplicationId,
            Origin = source.Origin,
            AutoCreatedReservation = source.AutoCreated,
            DeliveryModel = source.DeliveryModel,
            IsOnFlexiPaymentPilot = false
        };

        return result;
    }
}