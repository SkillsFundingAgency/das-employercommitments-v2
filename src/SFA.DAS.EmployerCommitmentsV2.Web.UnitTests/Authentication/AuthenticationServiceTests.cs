using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Authentication
{
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
            Assert.IsFalse(_fixture.Sut.IsUserAuthenticated());
        }

        [Test]
        public void WhenNotAuthenticated_ThenUserIdShouldReturnNull()
        {
            _fixture.NoAuthenticatedUser();
            Assert.IsNull(_fixture.Sut.UserId);
        }

        [Test]
        public void WhenNotAuthenticated_ThenUserNameShouldReturnNull()
        {
            _fixture.NoAuthenticatedUser();
            Assert.IsNull(_fixture.Sut.UserName);
        }

        [Test]
        public void WhenNotAuthenticated_ThenUserEmailShouldReturnNull()
        {
            _fixture.NoAuthenticatedUser();
            Assert.IsNull(_fixture.Sut.UserEmail);
        }

        [Test]
        public void WhenNotAuthenticated_ThenUserInfoShouldReturnNull()
        {
            _fixture.NoAuthenticatedUser();
            Assert.IsNull(_fixture.Sut.UserInfo);
        }

        [Test]
        public void WhenAuthenticated_ThenIsUserAuthenticatedShouldReturnTrue()
        {
            _fixture.SetAuthenticatedUser();
            Assert.IsTrue(_fixture.Sut.IsUserAuthenticated());
        }

        [Test]
        public void WhenAuthenticated_ThenUserIdShouldReturnTheUserId()
        {
            _fixture.SetAuthenticatedUser();
            Assert.AreEqual("UserId", _fixture.Sut.UserId);
        }

        [Test]
        public void WhenAuthenticated_ThenUserNameShouldReturnTheUserName()
        {
            _fixture.SetAuthenticatedUser();
            Assert.AreEqual("UserName", _fixture.Sut.UserName);
        }

        [Test]
        public void WhenAuthenticated_ThenUserEmailShouldReturnTheUserEmail()
        {
            _fixture.SetAuthenticatedUser();
            Assert.AreEqual("UserEmail", _fixture.Sut.UserEmail);
        }

        [Test]
        public void WhenAuthenticated_ThenUserInfoShouldReturnAllValues()
        {
            _fixture.SetAuthenticatedUser();
            Assert.IsNotNull(_fixture.Sut.UserInfo);
            Assert.AreEqual("UserId", _fixture.Sut.UserInfo.UserId);
            Assert.AreEqual("UserName", _fixture.Sut.UserInfo.UserDisplayName);
            Assert.AreEqual("UserEmail", _fixture.Sut.UserInfo.UserEmail);
        }

        [Test]
        public void WhenAuthenticatedButNoDasEmailClaim_ThenChecksClaimTypesEmail()
        {
            _fixture.SetNoDasEmailAuthenticatedUser();
            
            Assert.IsNotNull(_fixture.Sut.UserInfo);
            Assert.AreEqual("UserId", _fixture.Sut.UserInfo.UserId);
            Assert.AreEqual("UserName", _fixture.Sut.UserInfo.UserDisplayName);
            Assert.AreEqual("UserEmail", _fixture.Sut.UserInfo.UserEmail);
        }
        
        [Test]
        public void WhenAuthenticatedButNoNameClaim_ThenGetNameFromApiClient()
        {
            _fixture.SetNoDasEmailAuthenticatedUserNoName();
            
            Assert.IsNotNull(_fixture.Sut.UserInfo);
            Assert.AreEqual("UserId", _fixture.Sut.UserInfo.UserId);
            Assert.AreEqual("Test Last", _fixture.Sut.UserInfo.UserDisplayName);
            Assert.AreEqual("UserEmail", _fixture.Sut.UserInfo.UserEmail);
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
                   new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"), 
                   new Claim(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier, "UserName"), 
                   new Claim(EmployeeClaims.IdamsUserEmailClaimTypeIdentifier, "UserEmail"), 
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
                new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"), 
                new Claim(EmployeeClaims.IdamsUserDisplayNameClaimTypeIdentifier, "UserName"), 
                new Claim(ClaimTypes.Email, "UserEmail"), 
            }, "mock"));
            HttpContext = new DefaultHttpContext { User = user };
            HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

            return this;
        }
        public AuthenticationServiceTestsFixture SetNoDasEmailAuthenticatedUserNoName()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(EmployeeClaims.IdamsUserIdClaimTypeIdentifier, "UserId"), 
                new Claim(ClaimTypes.Email, "UserEmail"), 
            }, "mock"));
            HttpContext = new DefaultHttpContext { User = user };
            HttpContextAccessor.Setup(o => o.HttpContext).Returns(HttpContext);

            return this;
        }
        
    }
}