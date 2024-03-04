using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class DetailsViewModelToAcknowledgementRequestMapperTests
{
    private DetailsViewModelToAcknowledgementRequestMapper _mapper;
    private Mock<IApprovalsApiClient> _apiClient;

    private PostCohortDetailsRequest _apiRequestBody;
    private long _apiRequestAccountId;
    private long _apiRequestCohortId;
    private readonly Fixture _autoFixture = new();

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
        var request = _autoFixture.Build<DetailsViewModel>()
            .With(x => x.Selection, CohortDetailsOptions.Approve)
            .Create();

        await _mapper.Map(request);

        Assert.Multiple(() =>
        {
            Assert.That(_apiRequestAccountId, Is.EqualTo(request.AccountId));
            Assert.That(_apiRequestCohortId, Is.EqualTo(request.CohortId));
            Assert.That(_apiRequestBody.Message, Is.EqualTo(request.ApproveMessage));
            Assert.That(_apiRequestBody.SubmissionType, Is.EqualTo(PostCohortDetailsRequest.CohortSubmissionType.Approve));
        });
    }

    [Test]
    public async Task Cohort_Send_Is_Submitted_Correctly()
    {
        var request = _autoFixture.Build<DetailsViewModel>()
            .With(x => x.Selection, CohortDetailsOptions.Send)
            .Create();

        await _mapper.Map(request);

        Assert.Multiple(() =>
        {
            Assert.That(_apiRequestAccountId, Is.EqualTo(request.AccountId));
            Assert.That(_apiRequestCohortId, Is.EqualTo(request.CohortId));
            Assert.That(_apiRequestBody.Message, Is.EqualTo(request.SendMessage));
            Assert.That(_apiRequestBody.SubmissionType, Is.EqualTo(PostCohortDetailsRequest.CohortSubmissionType.Send));
        });
    }
}