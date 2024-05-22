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
    public async Task When_Calling_GetAllProviders_Then_The_Data_Is_Returned(GetProvidersListResponse response)
    {
        var outerApiClient = new Mock<IOuterApiClient>();
        var expectedUrl = $"providers";
        outerApiClient.Setup(x => x.Get<GetProvidersListResponse>(expectedUrl)).ReturnsAsync(response);
        var approvalsApiClient = new ApprovalsApiClient(outerApiClient.Object);

        var actual = await approvalsApiClient.GetAllProviders();

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


}