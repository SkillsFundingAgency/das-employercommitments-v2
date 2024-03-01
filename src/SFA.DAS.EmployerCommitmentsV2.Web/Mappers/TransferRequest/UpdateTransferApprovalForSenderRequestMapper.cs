using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;

public class UpdateTransferApprovalForSenderRequestMapper : IMapper<TransferRequestForSenderViewModel, UpdateTransferApprovalForSenderRequest>
{
    private readonly IAuthenticationService _authenticationService;
    protected readonly IEncodingService _encodingService;

    public UpdateTransferApprovalForSenderRequestMapper(IAuthenticationService authenticationService, IEncodingService encodingService)
    {
        _authenticationService = authenticationService;
        _encodingService = encodingService;
    }

    public async Task<UpdateTransferApprovalForSenderRequest> Map(TransferRequestForSenderViewModel source)
    {
        return await Task.FromResult(new UpdateTransferApprovalForSenderRequest
        {
            TransferSenderId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId),
            TransferRequestId = _encodingService.Decode(source.TransferRequestHashedId, EncodingType.TransferRequestId),
            CohortId = _encodingService.Decode(source.HashedCohortReference, EncodingType.CohortReference),
            TransferReceiverId = _encodingService.Decode(source.TransferReceiverPublicHashedAccountId, EncodingType.PublicAccountId),
            TransferApprovalStatus = source.ApprovalConfirmed.Value ? TransferApprovalStatus.Approved : TransferApprovalStatus.Rejected,
            UserInfo = _authenticationService.UserInfo
        });
    }
}