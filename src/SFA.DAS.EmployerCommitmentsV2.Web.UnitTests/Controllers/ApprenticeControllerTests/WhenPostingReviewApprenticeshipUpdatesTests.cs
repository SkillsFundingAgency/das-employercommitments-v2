using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingReviewApprenticeshipUpdatesTests
    {
        private WhenPostingReviewApprenticeshipUpdatesTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingReviewApprenticeshipUpdatesTestsFixture();
        }   

        [Test]
        public async Task Verify_AcceptApprenticeshipUpdates_IsCalled_When_UserSelectsTo_ApproveChanges()
        {
            _fixture.ViewModel.ApproveChanges = true;
             await _fixture.ReviewApprenticeshipUpdates();
            _fixture.VerifyAcceptApprenticeshipUpdatesApiIsCalled();
        }

        [Test]
        public async Task Verify_RejectApprenticeshipUpdates_IsCalled_When_UserSelectsTo_ApproveChanges()
        {
            _fixture.ViewModel.ApproveChanges = false;
            await _fixture.ReviewApprenticeshipUpdates();
            _fixture.VerifyRejectApprenticeshipUpdatesApiIsCalled();
        }

        [Test]
        public async Task VerifyIsRedirectedToApprenticeshipDetails()
        {
            _fixture.ViewModel.ApproveChanges = false;
            var response = await _fixture.ReviewApprenticeshipUpdates() as RedirectToActionResult;
            Assert.AreEqual("ApprenticeshipDetails", response.ActionName);  
        }
    }

    public class WhenPostingReviewApprenticeshipUpdatesTestsFixture : ApprenticeControllerTestFixtureBase
    {
        public const string FlashMessageBodyTempDataKey = "FlashMessageBody";
        public const string FlashMessageLevelTempDataKey = "FlashMessageLevel";
        public const string FlashMessageTitleTempDataKey = "FlashMessageTitle";
        private const string ChangesApprovedMessage = "Changes approved";
        private const string ChangesRejectedMessage = "Changes rejected";

        public ReviewApprenticeshipUpdatesViewModel ViewModel { get; set; }

        public UserInfo UserInfo;
        public Mock<IAuthenticationService> AuthenticationService { get; }
         
        public WhenPostingReviewApprenticeshipUpdatesTestsFixture() : base () 
        {
            ViewModel = new ReviewApprenticeshipUpdatesViewModel { ApprenticeshipId = 1, AccountId = 1 };
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());

            var autoFixture = new Fixture();
            UserInfo = autoFixture.Create<UserInfo>();
            AuthenticationService = new Mock<IAuthenticationService>();
            AuthenticationService.Setup(x => x.UserInfo).Returns(UserInfo);
        }

        public async Task<IActionResult> ReviewApprenticeshipUpdates()
        {
            return await _controller.ReviewApprenticeshipUpdates(AuthenticationService.Object, ViewModel);
        }     

        internal void VerifyAcceptApprenticeshipUpdatesApiIsCalled()
        {
            _mockCommitmentsApiClient
                .Verify(x => x.AcceptApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.Is<AcceptApprenticeshipUpdatesRequest>(o => o.UserInfo != null), It.IsAny<CancellationToken>()), Times.Once());
           
            Assert.IsTrue(_controller.TempData.Values.Contains(ChangesApprovedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageBodyTempDataKey));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevelTempDataKey));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageTitleTempDataKey));
        }

        internal void VerifyRejectApprenticeshipUpdatesApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.RejectApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.Is<RejectApprenticeshipUpdatesRequest>(o => o.UserInfo != null), It.IsAny<CancellationToken>()), Times.Once());
            Assert.IsTrue(_controller.TempData.Values.Contains(ChangesRejectedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageBodyTempDataKey));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevelTempDataKey));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageTitleTempDataKey));
        }
    }
}
