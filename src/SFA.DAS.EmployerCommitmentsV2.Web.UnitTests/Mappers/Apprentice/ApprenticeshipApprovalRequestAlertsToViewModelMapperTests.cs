using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestAlertsToViewModelMapperTests
{
    [Test, MoqAutoData]
    public async Task Then_Maps_MainValues(
        ApprenticeshipApprovalRequestAlertsRequest source,
        GetApprenticeshipApprovalRequestAlertsResponse apiResponse,
        [Frozen] Mock<IApprovalsApiClient> mockApprovalsApiClient,
        [Greedy] ApprenticeshipApprovalRequestAlertsToViewModelMapper mapper)
    {
        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequestAlerts(It.Is<GetApprovalRequestAlertRequest>(t => t.ApprenticeshipId == source.ApprenticeshipId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await mapper.Map(source);

        result.ApprenticeshipHashedId.Should().Be(source.ApprenticeshipHashedId);
        result.AccountHashedId.Should().Be(source.AccountHashedId);
        result.ApprenticeName.Should().Be(apiResponse.ApprenticeName);
    }

    [TestCase("TNP1", "1000", "2000", "Training price (TNP1)", "£1,000", "£2,000")]
    [TestCase("TNP1", "kkk", "2000", "Training price (TNP1)", "#error#", "£2,000")]
    [TestCase("TNP2", "9000", "2120", "Assessment price (TNP2)", "£9,000", "£2,120")]
    [TestCase("Unknown", "ABCD123", "XXXX", "Unknown", "ABCD123", "XXXX")]
    public async Task Then_Maps_Item_ToDisplayLine(string field, string oldValue, string newValue, string expectedName, string expectedOldValue, string expectedNewValue)
    {
        var fixture = new Fixture();

        var source = fixture.Create<ApprenticeshipApprovalRequestAlertsRequest>();
        var apiResponse = fixture.Build<GetApprenticeshipApprovalRequestAlertsResponse>()
            .With(x => x.ApprenticeName)
            .With(x => x.ApprovalRequests, new List<ApprovalRequestItem>
        {
            new() {
                Items = [ new ApprovalFieldRequest()
                {
                    Field = field,
                 Old = oldValue,
                New = newValue,
                 Status = 1,
                 Created = DateTime.UtcNow.Date}
                ]
            }
        }).Create();

        var mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequestAlerts(It.Is<GetApprovalRequestAlertRequest>(t => t.ApprenticeshipId == source.ApprenticeshipId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var mapper = new ApprenticeshipApprovalRequestAlertsToViewModelMapper(mockApprovalsApiClient.Object);

        var result = await mapper.Map(source);

        result.ApprovalRequests.First().ApprovalRequestFieldItems.First().Field.Should().Be(expectedName);
        result.ApprovalRequests.First().ApprovalRequestFieldItems.First().Old.Should().Be(expectedOldValue);
        result.ApprovalRequests.First().ApprovalRequestFieldItems.First().New.Should().Be(expectedNewValue);
    }
}