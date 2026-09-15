using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticesipApprovalRequestToViewModelMapperTests
{
    [Test, MoqAutoData]
    public async Task Then_Maps_MainValues(
        ApprenticeshipApprovalRequest source,
        GetApprenticeshipApprovalResponse apiResponse,
        [Frozen]Mock<IApprovalsApiClient> mockApprovalsApiClient,
        [Greedy] ApprenticeshipApprovalRequestToViewModelMapper mapper)
    {
        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await mapper.Map(source);

        result.ApprenticeshipHashedId.Should().Be(source.ApprenticeshipHashedId);
        result.AccountHashedId.Should().Be(source.AccountHashedId);
        result.ApprovalRequestId.Should().Be(source.ApprovalRequestId);
        result.ApprovalRequestStatus.Should().Be(apiResponse.ApprovalRequestStatus);
        result.Name.Should().Be(apiResponse.Name);
        result.ULN.Should().Be(apiResponse.ULN);
        result.CourseName.Should().Be(apiResponse.CourseName);
        result.ProviderName.Should().Be(apiResponse.ProviderName);
        result.UKPRN.Should().Be(apiResponse.UKPRN);
    }

    [TestCase("TNP1", "1000", "2000", "Training price (TNP1)", "£1,000", "£2,000")]
    [TestCase("TNP1", "kkk", "2000", "Training price (TNP1)", "#error#", "£2,000")]
    [TestCase("TNP2", "9000", "2120", "Assessment price (TNP2)", "£9,000", "£2,120")]
    [TestCase("Unknown", "ABCD123", "XXXX", "Unknown", "ABCD123", "XXXX")]
    public async Task Then_Maps_Item_ToDisplayLine(string field, string oldValue, string newValue, string expectedName, string expectedOldValue, string expectedNewValue)
    {
        var fixture = new Fixture();

        var source = fixture.Create<ApprenticeshipApprovalRequest>();
        var apiResponse = fixture.Build<GetApprenticeshipApprovalResponse>()
            .With(x => x.ApprovalRequestId, source.ApprovalRequestId)
            .With(x => x.ApprenticeshipId, source.ApprenticeshipId)
            .With(x => x.AccountId, source.AccountId)
            .With(x => x.Items, new List<GetApprenticeshipApprovalResponse.ChangeItem>
        {
            new GetApprenticeshipApprovalResponse.ChangeItem
            {
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue
            }
        }).Create();

        var mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        var mapper = new ApprenticeshipApprovalRequestToViewModelMapper(mockApprovalsApiClient.Object);

        var result = await mapper.Map(source);

        result.Items.First().FieldName.Should().Be(expectedName);
        result.Items.First().OldValue.Should().Be(expectedOldValue);
        result.Items.First().NewValue.Should().Be(expectedNewValue);
    }

    [TestCase(CocApprovalResultStatus.Superseded, true)]
    [TestCase(CocApprovalResultStatus.Cancelled, true)]
    [TestCase(CocApprovalResultStatus.Pending, false)]
    [TestCase(CocApprovalResultStatus.Complete, false)]
    public async Task Then_Maps_IsSupersededOrCancelled_Correctly(CocApprovalResultStatus status, bool expected)
    {
        var fixture = new Fixture();

        var source = fixture.Create<ApprenticeshipApprovalRequest>();
        var apiResponse = fixture.Build<GetApprenticeshipApprovalResponse>()
            .With(x => x.ApprovalRequestId, source.ApprovalRequestId)
            .With(x => x.ApprenticeshipId, source.ApprenticeshipId)
            .With(x => x.AccountId, source.AccountId)
            .With(x => x.ApprovalRequestStatus, status).Create();

        var mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);
        var mapper = new ApprenticeshipApprovalRequestToViewModelMapper(mockApprovalsApiClient.Object);

        var result = await mapper.Map(source);

        result.IsSupersededOrCancelled.Should().Be(expected);
    }
}