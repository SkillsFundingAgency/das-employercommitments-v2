using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Middleware;
using SFA.DAS.GovUK.Auth.Employer;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Middleware;

[TestFixture]
public class MissingClaimsMiddlewareTests
{
    private MissingClaimsMiddleware _middleware;
    private Mock<RequestDelegate> _nextDelegate;
    private Mock<IGovAuthEmployerAccountService> _userAccountServiceMock;

    [SetUp]
    public void SetUp()
    {
        _nextDelegate = new Mock<RequestDelegate>();
        _userAccountServiceMock = new Mock<IGovAuthEmployerAccountService>();
        _middleware = new MissingClaimsMiddleware(_nextDelegate.Object, _userAccountServiceMock.Object);
    }

    [Test]
    public async Task InvokeAsync_WithAuthenticatedUserAndMissingAccountClaims_ShouldEnrichWithAccountClaims()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection().AddSingleton(Mock.Of<IAuthenticationService>()).BuildServiceProvider()
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Email, "test@example.com")
        ], CookieAuthenticationDefaults.AuthenticationScheme));
        context.User = user;

        _userAccountServiceMock.Setup(x => x.GetUserAccounts("123", "test@example.com"))
            .ReturnsAsync(new EmployerUserAccounts
            {
                EmployerAccounts = new List<EmployerUserAccountItem>
                {
                    new() { AccountId = "456", Role = "owner" },
                    new() { AccountId = "789", Role = "transactor" }
                }
            });

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _userAccountServiceMock.Verify(x => x.GetUserAccounts("123", "test@example.com"), Times.Once);

        var enrichedUser = context.User;
        enrichedUser.Claims.Any(c => c.Type == EmployeeClaims.AccountIdClaimTypeIdentifier && c.Value == "456").Should().BeTrue();
        enrichedUser.Claims.Any(c => c.Type == EmployeeClaims.AccountIdClaimTypeIdentifier && c.Value == "789").Should().BeTrue();

        _nextDelegate.Verify(x => x(context), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithAuthenticatedUserAndNoMissingAccountClaims_ShouldNotEnrichWithAccountClaims()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(EmployeeClaims.AccountIdClaimTypeIdentifier, "456")
        ], CookieAuthenticationDefaults.AuthenticationScheme));
        context.User = user;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _userAccountServiceMock.Verify(x => x.GetUserAccounts(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _nextDelegate.Verify(x => x(context), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithUnauthenticatedUser_ShouldNotEnrichWithAccountClaims()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal();
        context.User = user;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _userAccountServiceMock.Verify(x => x.GetUserAccounts(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _nextDelegate.Verify(x => x(context), Times.Once);
    }
}