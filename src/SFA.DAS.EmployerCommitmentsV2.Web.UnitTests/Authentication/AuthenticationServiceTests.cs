using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;

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
                   new Claim(EmployeeClaims.Id, "UserId"), 
                   new Claim(EmployeeClaims.Name, "UserName"), 
                   new Claim(EmployeeClaims.Email, "UserEmail"), 
            }, "mock"));
 
            Sut = new AuthenticationService(HttpContextAccessor.Object);
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
    }
}