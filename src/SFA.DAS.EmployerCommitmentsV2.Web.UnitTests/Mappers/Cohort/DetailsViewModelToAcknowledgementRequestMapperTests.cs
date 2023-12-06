﻿using System.Threading;
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

        private PostCohortDetailsRequest _apiRequestBody;
        private long _apiRequestAccountId;
        private long _apiRequestCohortId;

        [SetUp]
        public void Setup()
        {
            _apiClient = new Mock<IApprovalsApiClient>();

            _apiClient.Setup(x => x.PostCohortDetails(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PostCohortDetailsRequest>(), It.IsAny<CancellationToken>()))
                .Callback((long accountId, long cohortId, PostCohortDetailsRequest request, CancellationToken cancellationToken) =>
                {
                    _apiRequestAccountId = accountId;
                    _apiRequestCohortId = cohortId;
                    _apiRequestBody = request;

                })
                .Returns(() => Task.CompletedTask);

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

            Assert.That(_apiRequestAccountId, Is.EqualTo(request.AccountId));
            Assert.That(_apiRequestCohortId, Is.EqualTo(request.CohortId));
            Assert.That(_apiRequestBody.Message, Is.EqualTo(request.ApproveMessage));
            Assert.That(_apiRequestBody.SubmissionType, Is.EqualTo(PostCohortDetailsRequest.CohortSubmissionType.Approve));
        }

        [Test]
        public async Task Cohort_Send_Is_Submitted_Correctly()
        {
            var request = new DetailsViewModel
            {
                Selection = CohortDetailsOptions.Send
            };

            await _mapper.Map(request);

            Assert.That(_apiRequestAccountId, Is.EqualTo(request.AccountId));
            Assert.That(_apiRequestCohortId, Is.EqualTo(request.CohortId));
            Assert.That(_apiRequestBody.Message, Is.EqualTo(request.SendMessage));
            Assert.That(_apiRequestBody.SubmissionType, Is.EqualTo(PostCohortDetailsRequest.CohortSubmissionType.Send));
        }
    }
}
