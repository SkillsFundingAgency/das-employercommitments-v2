using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Authentication;

[TestFixture]
[Parallelizable]
public class AuthenticationServiceTests
{
    private AuthenticationServiceTestsFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new AuthenticationServiceTestsFixture();
    }

    [Test]
    public void WhenNotAuthenticated_ThenIsUserAuthenticatedShouldReturnFalse()
    {
        _fixture.NoAuthenticatedUser();
        Assert.That(_fixture.Sut.IsUserAuthenticated(), Is.False);
    }

    [Test]
    public void WhenNotAuthenticated_ThenUserIdShouldReturnNull()
    {
        _fixture.NoAuthenticatedUser();
        Assert.That(_fixture.Sut.UserId, Is.Null);
    }

    [Test]
    public void WhenNotAuthenticated_ThenUserNameShouldReturnNull()
    {
        _fixture.NoAuthenticatedUser();
        Assert.That(_fixture.Sut.UserName, Is.Null);
    }

    [Test]
    public void WhenNotAuthenticated_ThenUserEmailShouldReturnNull()
    {
        _fixture.NoAuthenticatedUser();
        Assert.That(_fixture.Sut.UserEmail, Is.Null);
    }

    [Test]
    public void WhenNotAuthenticated_ThenUserInfoShouldReturnNull()
    {
        _fixture.NoAuthenticatedUser();
        Assert.That(_fixture.Sut.UserInfo, Is.Null);
    }

    [Test]
    public void WhenAuthenticated_ThenIsUserAuthenticatedShouldReturnTrue()
    {
        _fixture.SetAuthenticatedUser();
        Assert.That(_fixture.Sut.IsUserAuthenticated(), Is.True);
    }

    [Test]
    public void WhenAuthenticated_ThenUserIdShouldReturnTheUserId()
    {
        _fixture.SetAuthenticatedUser();
        Assert.That(_fixture.Sut.UserId, Is.EqualTo("UserId"));
    }

    [Test]
    public void WhenAuthenticated_ThenUserNameShouldReturnTheUserName()
    {
        _fixture.SetAuthenticatedUser();
        Assert.That(_fixture.Sut.UserName, Is.EqualTo("UserName"));
    }

    [Test]
    public void WhenAuthenticated_ThenUserEmailShouldReturnTheUserEmail()
    {
        _fixture.SetAuthenticatedUser();
        Assert.That(_fixture.Sut.UserEmail, Is.EqualTo("UserEmail"));
    }

    [Test]
    public void WhenAuthenticated_ThenUserInfoShouldReturnAllValues()
    {
        _fixture.SetAuthenticatedUser();
        Assert.That(_fixture.Sut.UserInfo, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_fixture.Sut.UserInfo.UserId, Is.EqualTo("UserId"));
            Assert.That(_fixture.Sut.UserInfo.UserDisplayName, Is.EqualTo("UserName"));
            Assert.That(_fixture.Sut.UserInfo.UserEmail, Is.EqualTo("UserEmail"));
        });
    }

    [Test]
    public void WhenAuthenticatedButNoDasEmailClaim_ThenChecksClaimTypesEmail()
    {
        _fixture.SetNoDasEmailAuthenticatedUser();

        Assert.That(_fixture.Sut.UserInfo, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_fixture.Sut.UserInfo.UserId, Is.EqualTo("UserId"));
            Assert.That(_fixture.Sut.UserInfo.UserDisplayName, Is.EqualTo("UserName"));
            Assert.That(_fixture.Sut.UserInfo.UserEmail, Is.EqualTo("UserEmail"));
        });
    }
        
    [Test]
    public void WhenAuthenticatedButNoNameClaim_ThenGetNameFromApiClient()
    {
        _fixture.SetNoDasEmailAuthenticatedUserNoName();

        Assert.That(_fixture.Sut.UserInfo, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_fixture.Sut.UserInfo.UserId, Is.EqualTo("UserId"));
            Assert.That(_fixture.Sut.UserInfo.UserDisplayName, Is.EqualTo("Test Last"));
            Assert.That(_fixture.Sut.UserInfo.UserEmail, Is.EqualTo("UserEmail"));
        });
    }
}

public class AuthenticationServiceTestsFixture
{
    public Mock<IHttpContextAccessor> HttpContextAccessor { get; }
    public HttpContext HttpContext { get; private set; }
    public ClaimsPrincipal User { get; }
    public AuthenticationService Sut { get; set; }

    public AuthenticationServiceTestsFixture()
    {
        HttpContextAccessor = new Mock<IHttpContextAccessor>();
        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"), 
            new(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier, "UserName"), 
            new(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier, "UserEmail"), 
        }, "mock"));

        var approvalsApiClient = new Mock<IApprovalsApiClient>();
        approvalsApiClient.Setup(x => x.GetEmployerUserAccounts("UserEmail", "UserId")).ReturnsAsync(
            new GetUserAccountsResponse
            {
                FirstName = "Test",
                LastName = "Last"
            });
            
        Sut = new AuthenticationService(HttpContextAccessor.Object, approvalsApiClient.Object);
    }

    public AuthenticationServiceTestsFixture NoAuthenticatedUser()
    {
        HttpContext = new DefaultHttpContext();
        HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

        return this;
    }

    public AuthenticationServiceTestsFixture SetAuthenticatedUser()
    {
        HttpContext = new DefaultHttpContext { User = User };
        HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

        return this;
    }

    public AuthenticationServiceTestsFixture SetNoDasEmailAuthenticatedUser()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"), 
            new(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier, "UserName"), 
            new(ClaimTypes.Email, "UserEmail"), 
        }, "mock"));
        HttpContext = new DefaultHttpContext { User = user };
        HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

        return this;
    }

    public AuthenticationServiceTestsFixture SetNoDasEmailAuthenticatedUserNoName()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"),
            new(ClaimTypes.Email, "UserEmail"),
        }, "mock"));
        HttpContext = new DefaultHttpContext { User = user };
        HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

        return this;
    }
}