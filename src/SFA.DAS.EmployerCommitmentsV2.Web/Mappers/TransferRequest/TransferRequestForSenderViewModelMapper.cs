﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching;
using SFA.DAS.EmployerCommitmentsV2.Services.LevyTransferMatching.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferRequestForSenderViewModelMapper : TransferRequestViewModelMapper<TransferRequestForSenderViewModel>, IMapper<TransferRequestRequest, TransferRequestForSenderViewModel>
    {
        public TransferRequestForSenderViewModelMapper(ICommitmentsApiClient commitmentsApiClient, ILevyTransferMatchingApiClient levyTransferMatchingApiClient, IEncodingService encodingService)
            : base(commitmentsApiClient, levyTransferMatchingApiClient, encodingService)
        {
        }

        public async Task<TransferRequestForSenderViewModel> Map(TransferRequestRequest source)
        {
            var transferRequestResponse = await _commitmentsApiClient.GetTransferRequestForSender(source.AccountId, source.TransferRequestId);
            var pledgeApplicationResponse = await _levyTransferMatchingApiClient.GetPledgeApplication(transferRequestResponse.PledgeApplicationId);

            var viewModel = Map(transferRequestResponse, pledgeApplicationResponse);
            return viewModel;
        }

        protected override TransferRequestForSenderViewModel Map(GetTransferRequestResponse transferRequestResponse, GetPledgeApplicationResponse getPledgeApplicationResponse)
        {
            var viewModel = base.Map(transferRequestResponse, getPledgeApplicationResponse);

            viewModel.TransferReceiverPublicHashedAccountId = _encodingService.Encode(transferRequestResponse.ReceivingEmployerAccountId, EncodingType.PublicAccountId);
            viewModel.TransferSenderHashedAccountId = _encodingService.Encode(transferRequestResponse.SendingEmployerAccountId, EncodingType.AccountId);
            viewModel.TransferReceiverName = transferRequestResponse.LegalEntityName;

            return viewModel;
        }
    }
}