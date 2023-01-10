using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class DetailsViewModelToAcknowledgementRequestMapperTests
    {
        private DetailsViewModelToAcknowledgementRequestMapper _mapper;
        private Mock<IApprovalsApiClient> _apiClient;

        [SetUp]
        public void Setup()
        {
            _apiClient = new Mock<IApprovalsApiClient>();

            _mapper = new DetailsViewModelToAcknowledgementRequestMapper(_apiClient.Object,
                Mock.Of<IAuthenticationService>());
        }

        [Test]
        public async Task Cohort_Approval_Is_Submitted_Correctly()
        {
            var request = new DetailsViewModel
            {
                Selection = CohortDetailsOptions.Approve
            };

            await _mapper.Map(request);

            _apiClient.Verify(x => x.PostCohortDetails(request.AccountId, request.CohortId, It.Is<PostCohortDetailsRequest>(r =>
                    r.SubmissionType == PostCohortDetailsRequest.CohortSubmissionType.Approve
                    && r.Message == request.ApproveMessage
                    ), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task Cohort_Send_Is_Submitted_Correctly()
        {
            var request = new DetailsViewModel
            {
                Selection = CohortDetailsOptions.Send
            };

            await _mapper.Map(request);

            _apiClient.Verify(x => x.PostCohortDetails(request.AccountId, request.CohortId, It.Is<PostCohortDetailsRequest>(r =>
                    r.SubmissionType == PostCohortDetailsRequest.CohortSubmissionType.Send
                    && r.Message == request.SendMessage
                ), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
