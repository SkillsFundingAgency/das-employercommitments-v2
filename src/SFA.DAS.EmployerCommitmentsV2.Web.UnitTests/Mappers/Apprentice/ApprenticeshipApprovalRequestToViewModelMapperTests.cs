using FluentAssertions;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticeshipApprovalRequestToViewModelMapperTests
{
    [Test, MoqAutoData]
    public async Task Then_Maps_MainValues(
        ApprenticeshipApprovalRequest source,
        GetApprenticeshipApprovalResponse apiResponse,
        GetEditApprenticeshipResponse apprenticeshipResponse,
        Mock<IApprovalsApiClient> mockApprovalsApiClient)
    {
        var mapper = new ApprenticeshipApprovalRequestToViewModelMapper(mockApprovalsApiClient.Object);

        mockApprovalsApiClient.Setup(s => s.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId))
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
    }

    [TestCase("TNP1", "1000", "2000", ApprenticeshipStatus.Live, true)]
    [TestCase("TNP1", "1000", "2000", ApprenticeshipStatus.Stopped, false)]
    [TestCase("TNP1", "kkk", "2000", ApprenticeshipStatus.Paused, true)]
    [TestCase("TNP2", "9000", "2120", ApprenticeshipStatus.WaitingToStart, true)]
    [TestCase("TNP2", "9000", "2120", ApprenticeshipStatus.Unknown, false)]
    [TestCase("FirstName", "James", "Nick", ApprenticeshipStatus.Live, true)]
    [TestCase("LastName", "Jameson", "Carr", ApprenticeshipStatus.Stopped, false)]
    public async Task Then_Maps_ApprenticeshipStatus(string field, string oldValue, string newValue, ApprenticeshipStatus apprenticeshipStatus, bool priceChangeApprovalAllowed)
    {
        var fixture = new Fixture();

        var source = fixture.Create<ApprenticeshipApprovalRequest>();
        var apiResponse = fixture.Build<GetApprenticeshipApprovalResponse>()
            .With(x => x.ApprovalRequestId, source.ApprovalRequestId)
            .With(x => x.ApprenticeshipId, source.ApprenticeshipId)
            .With(x => x.ApprenticeshipStatus, apprenticeshipStatus)
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
        result.ChangeApprovalAllowed.Should().Be(priceChangeApprovalAllowed);
    }

    [TestCase("TNP1", "1000", "2000", "Training price (TNP1)", "£1,000", "£2,000" )]
    [TestCase("TNP1", "1000", "2000", "Training price (TNP1)", "£1,000", "£2,000" )]
    [TestCase("TNP1", "kkk", "2000", "Training price (TNP1)", "#error#", "£2,000" )]
    [TestCase("TNP2", "9000", "2120", "Assessment price (TNP2)", "£9,000", "£2,120" )]
    [TestCase("TNP2", "9000", "2120", "Assessment price (TNP2)", "£9,000", "£2,120" )]
    [TestCase("Unknown", "ABCD123", "XXXX", "Unknown", "ABCD123", "XXXX" )]
    [TestCase("Unknown", "ABCD123", "XXXX", "Unknown", "ABCD123", "XXXX" )]
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

    [Test]
    public async Task Then_Maps_Prices_To_The_Latest_Total_Cost()
    {
        var oldSchedule = """
            [{"effectiveFrom":"2024-08-01","cost":900,"trainingPrice":null,"assessmentPrice":null},{"effectiveFrom":"2025-06-01","cost":1500,"trainingPrice":1000,"assessmentPrice":500}]
            """;
        var newSchedule = """
            [{"effectiveFrom":"2025-06-01","cost":1500,"trainingPrice":1000,"assessmentPrice":500},{"effectiveFrom":"2026-08-01","cost":1900.50,"trainingPrice":1500,"assessmentPrice":400.50}]
            """;

        var result = await MapItems(
            new GetApprenticeshipApprovalResponse.ChangeItem
            {
                FieldName = "PlannedEndDate",
                OldValue = "2026-06-01",
                NewValue = "2026-12-01"
            },
            new GetApprenticeshipApprovalResponse.ChangeItem
            {
                FieldName = "Prices",
                OldValue = oldSchedule,
                NewValue = newSchedule
            });

        result.Should().HaveCount(2);

        var endDate = result.First();
        endDate.FieldName.Should().Be("PlannedEndDate");
        endDate.OldValue.Should().Be("2026-06-01");
        endDate.NewValue.Should().Be("2026-12-01");
        endDate.NewValueHint.Should().BeNull();

        var price = result.Last();
        price.FieldName.Should().Be("Total cost");
        price.OldValue.Should().Be("£1,500");
        price.NewValue.Should().Be("£1,900.50");
        price.NewValueHint.Should().Be("From 1 August 2026");
    }

    [Test]
    public async Task Then_Unreadable_Prices_Show_As_An_Error()
    {
        var result = await MapItems(new GetApprenticeshipApprovalResponse.ChangeItem
        {
            FieldName = "Prices",
            OldValue = "not-json",
            NewValue = "[]"
        });

        var price = result.Should().ContainSingle().Subject;
        price.FieldName.Should().Be("Total cost");
        price.OldValue.Should().Be("#error#");
        price.NewValue.Should().Be("#error#");
        price.NewValueHint.Should().BeNull();
    }

    private static async Task<ICollection<ApprenticeshipApprovalRequestViewModel.ChangeItem>> MapItems(
        params GetApprenticeshipApprovalResponse.ChangeItem[] items)
    {
        var fixture = new Fixture();
        var source = fixture.Create<ApprenticeshipApprovalRequest>();
        var apiResponse = fixture.Build<GetApprenticeshipApprovalResponse>()
            .With(x => x.ApprovalRequestId, source.ApprovalRequestId)
            .With(x => x.ApprenticeshipId, source.ApprenticeshipId)
            .With(x => x.AccountId, source.AccountId)
            .With(x => x.Items, items.ToList())
            .Create();

        var mockApprovalsApiClient = new Mock<IApprovalsApiClient>();
        mockApprovalsApiClient
            .Setup(s => s.GetApprenticeshipApprovalRequest(source.AccountId, source.ApprenticeshipId, source.ApprovalRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var mapper = new ApprenticeshipApprovalRequestToViewModelMapper(mockApprovalsApiClient.Object);
        var result = await mapper.Map(source);
        return result.Items;
    }
}