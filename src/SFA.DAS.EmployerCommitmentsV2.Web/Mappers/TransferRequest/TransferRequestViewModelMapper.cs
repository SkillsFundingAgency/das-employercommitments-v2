using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;

public class TransferRequestViewModelMapper<T> where T : TransferRequestViewModel, new()
{
    protected readonly ICommitmentsApiClient _commitmentsApiClient;
    protected readonly IApprovalsApiClient _approvalsApiClient;
    protected readonly IEncodingService _encodingService;

    public TransferRequestViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IApprovalsApiClient approvalsApiClient, IEncodingService encodingService)
    {
        _commitmentsApiClient = commitmentsApiClient;
        _approvalsApiClient = approvalsApiClient;
        _encodingService = encodingService;
    }

    protected virtual T Map(GetTransferRequestResponse transferRequestResponse, GetPledgeApplicationResponse getPledgeApplicationResponse)
    {
        return new T
        {
            HashedCohortReference = _encodingService.Encode(transferRequestResponse.CommitmentId, EncodingType.CohortReference),
            TrainingList = transferRequestResponse.TrainingList?.Select(MapTrainingCourse).ToList() ?? new List<TrainingCourseSummaryViewModel>(),
            TransferApprovalStatusDesc = transferRequestResponse.Status.ToString(),
            TransferApprovalStatus = transferRequestResponse.Status,
            TransferApprovalSetBy = transferRequestResponse.ApprovedOrRejectedByUserName,
            TransferApprovalSetOn = transferRequestResponse.ApprovedOrRejectedOn,
            TotalCost = transferRequestResponse.TransferCost,
            FundingCap = transferRequestResponse.FundingCap,
            ShowFundingCapWarning = (transferRequestResponse.Status == TransferApprovalStatus.Pending
                                     || transferRequestResponse.Status == TransferApprovalStatus.Approved)
                                    && transferRequestResponse.TransferCost < transferRequestResponse.FundingCap,
            AutoApprovalEnabled = transferRequestResponse.AutoApproval,
            HashedPledgeId = getPledgeApplicationResponse == null ? string.Empty : _encodingService.Encode(getPledgeApplicationResponse.PledgeId, EncodingType.PledgeId),
            HashedPledgeApplicationId = !transferRequestResponse.PledgeApplicationId.HasValue ? string.Empty : _encodingService.Encode(transferRequestResponse.PledgeApplicationId.Value, EncodingType.PledgeApplicationId)
        };
    }

    private static TrainingCourseSummaryViewModel MapTrainingCourse(TrainingCourseSummary trainingCourseSummary)
    {
        return new TrainingCourseSummaryViewModel
        {
            ApprenticeshipCount = trainingCourseSummary.ApprenticeshipCount,
            CourseTitle = trainingCourseSummary.CourseTitle
        };
    }
}