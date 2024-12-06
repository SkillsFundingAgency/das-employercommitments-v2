using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Services.Approvals;

public class ApprovalsApiClientTests
{
    [Test, AutoData]
    public async Task Then_The_Data_Is_Returned_From_The_Encoded_Url(string email, string userId, GetUserAccountsResponse response)
    {
        email = $"{email}@email.com";
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"AccountUsers/{userId}/accounts?email={HttpUtility.UrlEncode(email)}";
        outerApiClient.Setup(x => x.Get<GetUserAccountsResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);
        
        var actual = await approvalsApiClient.GetEmployerUserAccounts(email, userId);
        
        actual.Should().BeEquivalentTo(response);
    }
    [Test, AutoData]

    public async Task When_Calling_GetAccountLegalEntity_Then_The_Data_Is_Returned(long id, GetAccountLegalEntityResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"accountlegalentity/{id}";
        outerApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetAccountLegalEntity(id);

        actual.Should().BeEquivalentTo(response);
    }

    [Test, AutoData]
    public async Task When_Calling_GetSelectProviderDetails_Then_The_Data_Is_Returned(long accountId, long accountLegalEntityid, GetSelectProviderDetailsResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"{accountId}/unapproved/add/select-provider?accountLegalEntityId={accountLegalEntityid}";
        outerApiClient.Setup(x => x.Get<GetSelectProviderDetailsResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetSelectProviderDetails(accountId, accountLegalEntityid);

        actual.Should().BeSameAs(response);
    }


    [Test, AutoData]
    public async Task When_Calling_GetSelectFundingOptions_Then_The_Data_Is_Returned(long accountId, GetSelectFundingOptionsResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"{accountId}/unapproved/add/select-funding";
        outerApiClient.Setup(x => x.Get<GetSelectFundingOptionsResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetSelectFundingOptions(accountId);

        actual.Should().BeSameAs(response);
    }

    [Test, AutoData]
    public async Task When_Calling_GetSelectDirectTransferConnections_Then_The_Data_Is_Returned(long accountId, GetSelectDirectTransferConnectionResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"{accountId}/unapproved/add/select-funding/select-direct-connection";
        outerApiClient.Setup(x => x.Get<GetSelectDirectTransferConnectionResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetSelectDirectTransferConnection(accountId);

        actual.Should().BeSameAs(response);
    }

    [Test, AutoData]
    public async Task When_Calling_GetSelectLevyTransferConnections_Then_The_Data_Is_Returned(long accountId, GetSelectLevyTransferConnectionResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"{accountId}/unapproved/add/select-funding/select-accepted-levy-connection";
        outerApiClient.Setup(x => x.Get<GetSelectLevyTransferConnectionResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetSelectLevyTransferConnection(accountId);

        actual.Should().BeSameAs(response);
    }
}