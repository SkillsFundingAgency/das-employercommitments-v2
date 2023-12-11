using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Configuration;
using SFA.DAS.EmployerCommitmentsV2.Models.UserAccounts;
using SFA.DAS.EmployerCommitmentsV2.Services;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Handlers;

public class WhenPopulatingAccountClaims
{
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<EmployerCommitmentsV2Configuration> commitmentsConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.IsSuspended = true;
        commitmentsConfiguration.Object.UseGovSignIn = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Value.Should().Be(accountData.FirstName + " " + accountData.LastName);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should().Be(emailAddress);
        actual.First(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Value.Should().Be("Suspended");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User_With_No_Accounts(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<EmployerCommitmentsV2Configuration> commitmentsConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.IsSuspended = true;
        accountData.EmployerAccounts = new List<EmployerUserAccountItem>();
        accountData.FirstName = null;
        accountData.LastName = null;
        commitmentsConfiguration.Object.UseGovSignIn = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Never);
        actual.Should().Contain(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));
        actual.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier))?.Value.Should().Be("{}");

        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
        actual.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier))?.Value.Should().BeNull();
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should().Be(emailAddress);
        actual.First(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Value.Should().Be("Suspended");
    }

    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User_And_Not_Suspended_Set(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<EmployerCommitmentsV2Configuration> commitmentsConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.IsSuspended = false;
        commitmentsConfiguration.Object.UseGovSignIn = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Value.Should().Be(accountData.FirstName + " " + accountData.LastName);
        actual.First(c => c.Type.Equals(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should().Be(emailAddress);
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.AuthorizationDecision))?.Value.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_EmployerUsers_User(
        string nameIdentifier,
        string idamsIdentifier,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] EmployerCommitmentsV2Configuration commitmentsConfiguration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, string.Empty);
        accountService.Setup(x => x.GetUserAccounts(idamsIdentifier, "")).ReturnsAsync(accountData);
        commitmentsConfiguration.UseGovSignIn = false;

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, string.Empty), Times.Never);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, string.Empty), Times.Once);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployeeClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
        actual.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.IdamsUserIdClaimTypeIdentifier)).Should().NotBeNull();
        actual.FirstOrDefault(c => c.Type.Equals(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Should().BeNull();
    }

    private static TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string idamsIdentifier, string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, nameIdentifier),
            new(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, idamsIdentifier),
            new(ClaimTypes.Email, emailAddress),
            new(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier, emailAddress)
        });

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
        return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",", "", typeof(TestAuthHandler)),
            new OpenIdConnectOptions(), Mock.Of<ClaimsPrincipal>(), new AuthenticationProperties())
        {
            Principal = claimsPrincipal
        };
    }


    private class TestAuthHandler : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}