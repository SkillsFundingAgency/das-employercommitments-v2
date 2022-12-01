using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Authentication;

public class EmployerUserAccountPostAuthenticationHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Passed_To_The_Api_And_Id_FirstName_LastName_Populated(
        string userId, string email,
        GetUserAccountsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        EmployerUserAccountPostAuthenticationHandler handler)
    {
        var tokenValidatedContext = ArrangeTokenValidatedContext(userId, email);
        approvalsApiClient.Setup(x => x.GetEmployerUserAccounts(email, userId)).ReturnsAsync(response);
            
        var actual = (await handler.GetClaims(tokenValidatedContext)).ToList();

        actual.First(c=>c.Type.Equals(EmployeeClaims.Id)).Value.Should().Be(response.EmployerUserId);
        actual.First(c=>c.Type.Equals(EmployeeClaims.Email)).Value.Should().Be(email);
        actual.First(c=>c.Type.Equals(EmployeeClaims.Name)).Value.Should().Be(response.FirstName + " " + response.LastName);
    }
    
    private TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
            new Claim(ClaimTypes.Email, emailAddress)
        });
        
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
        return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",","", typeof(TestAuthHandler)),
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

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}