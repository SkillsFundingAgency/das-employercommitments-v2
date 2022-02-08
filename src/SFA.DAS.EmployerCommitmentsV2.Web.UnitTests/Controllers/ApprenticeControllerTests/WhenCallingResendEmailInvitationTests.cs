using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

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

            _fixture.VerifyResendApprenticeshipInvitationApiIsCalled();            
            _fixture.VerifyRedirect(result);
        }
    }

    public class WhenCallingResendEmailInvitationTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private ResendEmailInvitationRequest _request;
        public UserInfo UserInfo;
        public Mock<IAuthenticationService> AuthenticationService { get; }

        public WhenCallingResendEmailInvitationTestsFixture() : base()
        {
            _request = _autoFixture.Create<ResendEmailInvitationRequest>();

            UserInfo = new Fixture().Create<UserInfo>();
            AuthenticationService = new Mock<IAuthenticationService>();
            AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);
        }

        public async Task<IActionResult> ResendEmailInvitation()
        {
            return await _controller.ResendEmailInvitation(AuthenticationService.Object, _request);
        }

        public void VerifyResendApprenticeshipInvitationApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.ResendApprenticeshipInvitation(
                _request.ApprenticeshipId, It.Is<SaveDataRequest>(o => o.UserInfo != null), It.IsAny<CancellationToken>()), Times.Once());
        }

        public void VerifyRedirect(IActionResult result)
        {
            result.VerifyReturnsRedirectToActionResult().WithActionName("ApprenticeshipDetails");

            var redirect = result as RedirectToActionResult;
            
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], _request.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], _request.ApprenticeshipHashedId);
        }
    }
}
