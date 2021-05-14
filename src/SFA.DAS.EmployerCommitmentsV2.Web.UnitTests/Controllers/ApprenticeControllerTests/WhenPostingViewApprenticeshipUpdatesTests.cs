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
    public class WhenPostingViewApprenticeshipUpdatesTests
    {
        private WhenPostingViewApprenticeshipUpdatesTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenPostingViewApprenticeshipUpdatesTestsFixture();
        }   

        [Test]
        public async Task Verify_UndoApprenticeshipUpdates_IsCalled_When_UserSelectsTo_UndoChanges()
        {
            _fixture.ViewModel.UndoChanges = true;
             await _fixture.ViewApprenticeshipUpdates();
            _fixture.Verify_UndoApprenticeshipUpdatesApi_Is_Called();
        }

        [Test]
        public async Task Verify_UndoApprenticeshipUpdates_IsNotCalled_When_UserSelects_Not_To_UndoChanges()
        {
            _fixture.ViewModel.UndoChanges = false;
            await _fixture.ViewApprenticeshipUpdates();
            _fixture.Verify_UndoApprenticeshipUpdatesApi_IsNot_Called();
        }


        [Test]
        public async Task VerifyIsRedirectedToApprenticeshipDetails()
        {
            _fixture.ViewModel.UndoChanges = false;
            var response = await _fixture.ViewApprenticeshipUpdates() as RedirectToActionResult;
            Assert.AreEqual("ApprenticeshipDetails", response.ActionName);  
        }
    }

    public class WhenPostingViewApprenticeshipUpdatesTestsFixture : ApprenticeControllerTestFixtureBase
    {
        public ViewApprenticeshipUpdatesRequestViewModel ViewModel { get; set; }
        public WhenPostingViewApprenticeshipUpdatesTestsFixture() : base () 
        {
            ViewModel = new ViewApprenticeshipUpdatesRequestViewModel { ApprenticeshipId = 1, AccountId = 1 };
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> ViewApprenticeshipUpdates()
        {
            return await _controller.ViewApprenticeshipUpdates(ViewModel);
        }     

        internal void Verify_UndoApprenticeshipUpdatesApi_Is_Called()
        {
            _mockCommitmentsApiClient.Verify(x => x.UndoApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.IsAny<UndoApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        internal void Verify_UndoApprenticeshipUpdatesApi_IsNot_Called()
        {
            _mockCommitmentsApiClient.Verify(x => x.UndoApprenticeshipUpdates(ViewModel.ApprenticeshipId, It.IsAny<UndoApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
