using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using System.Threading.Tasks;
using System.Threading;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingResendEmailInvitationTests
    {
        WhenCallingResendEmailInvitationTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingResendEmailInvitationTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.ResendEmailInvitation();

            result.VerifyReturnsRedirectToActionResult();

            _fixture.VerifyRedirect(result as RedirectToActionResult);
        }
    }

    public class WhenCallingResendEmailInvitationTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ResendEmailInvitationRequest _request;
        public UserInfo UserInfo;
        public Mock<IAuthenticationService> AuthenticationService { get; }

        public WhenCallingResendEmailInvitationTestsFixture() : base()
        {
            _request = _autoFixture.Create<ResendEmailInvitationRequest>();

            var autoFixture = new Fixture();
            UserInfo = autoFixture.Create<UserInfo>();
            AuthenticationService = new Mock<IAuthenticationService>();
            AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);
        }

        public async Task<IActionResult> ResendEmailInvitation()
        {
            return await _controller.ResendEmailInvitation(AuthenticationService.Object, _request);
        }

        public void VerifyRedirect(RedirectToActionResult redirect)
        {
            Assert.AreEqual(redirect.ActionName, "ApprenticeshipDetails");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], _request.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], _request.ApprenticeshipHashedId);
        }
    }
}
