using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest
{
    public class TransferRequestViewModelMapper<T> where T : TransferRequestViewModel, new()
    {
        protected readonly ICommitmentsApiClient _commitmentsApiClient;
        protected readonly IEncodingService _encodingService;

        public TransferRequestViewModelMapper(ICommitmentsApiClient commitmentsApiClient, IEncodingService encodingService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _encodingService = encodingService;
        }

        protected virtual T Map(GetTransferRequestResponse response)
        {
            return new T
            {
                HashedCohortReference = _encodingService.Encode(response.CommitmentId, EncodingType.CohortReference),
                TrainingList = response.TrainingList?.Select(MapTrainingCourse).ToList() ?? new List<TrainingCourseSummaryViewModel>(),
                TransferApprovalStatusDesc = response.Status.ToString(),
                TransferApprovalStatus = response.Status,
                TransferApprovalSetBy = response.ApprovedOrRejectedByUserName,
                TransferApprovalSetOn = response.ApprovedOrRejectedOn,
                TotalCost = response.TransferCost,
                FundingCap = response.FundingCap,
                ShowFundingCapWarning = (response.Status == TransferApprovalStatus.Pending
                                         || response.Status == TransferApprovalStatus.Approved)
                                         && response.TransferCost < response.FundingCap
            };
        }

        private TrainingCourseSummaryViewModel MapTrainingCourse(TrainingCourseSummary trainingCourseSummary)
        {
            return new TrainingCourseSummaryViewModel
            {
                ApprenticeshipCount = trainingCourseSummary.ApprenticeshipCount,
                CourseTitle = trainingCourseSummary.CourseTitle
            };
        }
    }
}