using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;

public class SelectTransferConnectionViewModelMapper(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    : IMapper<AddApprenticeshipCacheModel, SelectTransferConnectionViewModel>
{
    public async Task<SelectTransferConnectionViewModel> Map(AddApprenticeshipCacheModel source)
    {
        var result = await approvalsApiClient.GetSelectDirectTransferConnection(source.AccountId);

        return new SelectTransferConnectionViewModel
        {
            AccountHashedId = source.AccountHashedId,
            IsLevyAccount = result.IsLevyAccount,
            ApprenticeshipSessionKey = source.ApprenticeshipSessionKey,
            TransferConnections = result.TransferConnections == null ? new List<TransferConnection>() :
                result.TransferConnections.Select(x => new TransferConnection
                {
                    FundingEmployerAccountId = x.FundingEmployerAccountId,
                    FundingEmployerPublicHashedAccountId = encodingService.Encode(x.FundingEmployerAccountId, EncodingType.PublicAccountId),
                    FundingEmployerHashedAccountId = encodingService.Encode(x.FundingEmployerAccountId, EncodingType.AccountId),
                    FundingEmployerAccountName = x.FundingEmployerAccountName,
                    ApprovedOn = x.ApprovedOn
                }).ToList()
        };
    }
}