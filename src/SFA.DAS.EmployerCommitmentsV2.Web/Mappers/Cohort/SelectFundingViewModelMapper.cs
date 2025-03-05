using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectFundingViewModelMapper(IApprovalsApiClient outerApiClient, ILogger<SelectFundingViewModelMapper> logger)
    : IMapper<AddApprenticeshipCacheModel, SelectFundingViewModel>
{
    public async Task<SelectFundingViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var selectFundingDetails = await outerApiClient.GetSelectFundingOptions(source.AccountId);

        logger.LogInformation("Funding Options Levy {IsLevyAccount}, Direct {HasDirectTransfersAvailable}, Additional Res {HasAdditionalReservationFundsAvailable}, Unallocated Res {HasUnallocatedReservationsAvailable}", 
            selectFundingDetails.IsLevyAccount,
            selectFundingDetails.HasDirectTransfersAvailable,
            selectFundingDetails.HasAdditionalReservationFundsAvailable,
            selectFundingDetails.HasUnallocatedReservationsAvailable);

        return new SelectFundingViewModel
        {
            AccountHashedId = source.AccountHashedId,           
            IsLevyAccount = selectFundingDetails.IsLevyAccount,
            HasDirectTransfersAvailable = selectFundingDetails.HasDirectTransfersAvailable,
            HasLtmTransfersAvailable = selectFundingDetails.HasLtmTransfersAvailable,
            HasUnallocatedReservationsAvailable = selectFundingDetails.HasUnallocatedReservationsAvailable,
            HasAdditionalReservationFundsAvailable = selectFundingDetails.HasAdditionalReservationFundsAvailable,
            FundingType = source.FundingType,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey
        };
    }
}