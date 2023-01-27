using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort
{
    public class DetailsViewModelToAcknowledgementRequestMapper : IMapper<DetailsViewModel, AcknowledgementRequest>
    {
        private readonly IApprovalsApiClient _apiClient;
        private readonly IAuthenticationService _authenticationService;

        public DetailsViewModelToAcknowledgementRequestMapper(IApprovalsApiClient apiClient, IAuthenticationService authenticationService)
        {
            _apiClient = apiClient;
            _authenticationService = authenticationService;
        }

        public async Task<AcknowledgementRequest> Map(DetailsViewModel source)
        {
            var apiRequest = new PostCohortDetailsRequest
            {
                SubmissionType = source.Selection == CohortDetailsOptions.Approve
                    ? PostCohortDetailsRequest.CohortSubmissionType.Approve
                    : PostCohortDetailsRequest.CohortSubmissionType.Send,
                Message = source.SendMessage,
                UserInfo = new ApimUserInfo
                {
                    UserDisplayName = _authenticationService.UserName,
                    UserEmail = _authenticationService.UserEmail,
                    UserId = _authenticationService.UserId
                }
            };

            await _apiClient.PostCohortDetails(source.AccountId, source.CohortId, apiRequest);

            return new AcknowledgementRequest
            {
                CohortReference = source.CohortReference,
                AccountHashedId = source.AccountHashedId
            };
        }
    }
}
