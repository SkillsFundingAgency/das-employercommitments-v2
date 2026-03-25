using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;

public class ChangeStatusRequestToViewModelMapper : IMapper<ChangeStatusRequest, ChangeStatusRequestViewModel>
{
    private readonly IApprovalsApiClient _approvalsApiClient;
    private readonly IEncodingService _encodingService;

    public ChangeStatusRequestToViewModelMapper(IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    {
        _approvalsApiClient = approvalsApiClient;
        _encodingService = encodingService;
    }

    public async Task<ChangeStatusRequestViewModel> Map(ChangeStatusRequest source)
    {
        var accountId = _encodingService.Decode(source.AccountHashedId, EncodingType.AccountId);
        var apprenticeship = await _approvalsApiClient.GetEditApprenticeship(accountId, source.ApprenticeshipId);

        var result = new ChangeStatusRequestViewModel
        {
            AccountHashedId = source.AccountHashedId,
            ApprenticeshipHashedId = source.ApprenticeshipHashedId,
            ApprenticeshipId = source.ApprenticeshipId,
            CurrentStatus = apprenticeship.Status,
            LearningType = apprenticeship.LearningType
        };

        return result;
    }
}