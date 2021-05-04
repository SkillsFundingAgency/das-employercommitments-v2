using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
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
        public ReviewApprenticeshipUpdatesRequestViewModel ViewModel { get; set; }
        public WhenPostingReviewApprenticeshipUpdatesTestsFixture() : base () 
        {
            ViewModel = new ReviewApprenticeshipUpdatesRequestViewModel { ApprenticeshipId = 1, AccountId = 1 };
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> ReviewApprenticeshipUpdates()
        {
            return await _controller.ReviewApprenticeshipUpdates(ViewModel);
        }     

        internal void VerifyAcceptApprenticeshipUpdatesApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.AcceptApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.IsAny<AcceptApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        internal void VerifyRejectApprenticeshipUpdatesApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.RejectApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.IsAny<RejectApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
