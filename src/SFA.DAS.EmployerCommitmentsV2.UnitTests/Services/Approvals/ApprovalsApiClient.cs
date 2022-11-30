using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.UnitTests.Services.Approvals
{
    
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
    }
}
