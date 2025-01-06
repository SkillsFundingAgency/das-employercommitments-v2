using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectFundingViewModelMapper : IMapper<AddApprenticeshipCacheModel, SelectFundingViewModel>
{
    private readonly IApprovalsApiClient _outerApiClient;
    private readonly ILogger<SelectFundingViewModelMapper> _logger;

    public SelectFundingViewModelMapper(IApprovalsApiClient outerApiClient, ILogger<SelectFundingViewModelMapper> logger)
    {
        _outerApiClient = outerApiClient;
        _logger = logger;
    }

    public async Task<SelectFundingViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var selectFundingDetails = await _outerApiClient.GetSelectFundingOptions(source.AccountId);

        _logger.LogInformation("Funding Options Levy {0}, Direct {1}, Additional Res {2}, Unallocated Res {3}", selectFundingDetails.IsLevyAccount,
            selectFundingDetails.HasDirectTransfersAvailable,
            selectFundingDetails.HasAdditionalReservationFundsAvailable,
            selectFundingDetails.HasUnallocatedReservationsAvailable);

        return new SelectFundingViewModel
        {
            AccountHashedId = source.AccountHashedId,           
            IsLevyAccount = selectFundingDetails.IsLevyAccount,
            HasDirectTransfersAvailable = selectFundingDetails.HasDirectTransfersAvailable,
            HasUnallocatedReservationsAvailable = selectFundingDetails.HasUnallocatedReservationsAvailable,
            HasAdditionalReservationFundsAvailable = selectFundingDetails.HasAdditionalReservationFundsAvailable,
            FundingType = source.FundingType,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }
}