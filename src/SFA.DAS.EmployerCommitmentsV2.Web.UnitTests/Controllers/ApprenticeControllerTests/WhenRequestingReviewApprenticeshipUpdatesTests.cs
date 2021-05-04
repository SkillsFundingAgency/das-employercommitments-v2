using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingReviewApprenticeshipUpdatesTests
    {
        private WhenRequestingReviewApprenticeshipUpdatesTestsixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingReviewApprenticeshipUpdatesTestsixture();
        }   

        [Test]
        public async Task VerifyViewModelMapperIsCalled()
        {
             await _fixture.ReviewApprenticeshipUpdates();
            _fixture.VerifyViewModelMapperIsCalled();
        }

        [Test]
        public async Task VerifyViewIsReturned()
        {
           var result =  await _fixture.ReviewApprenticeshipUpdates();
            _fixture.VerifyViewResultIsReturned(result);
        }
    }

    public class WhenRequestingReviewApprenticeshipUpdatesTestsixture : ApprenticeControllerTestFixtureBase
    {
        public ReviewApprenticehipUpdatesRequest Request { get; set; }
        public WhenRequestingReviewApprenticeshipUpdatesTestsixture() : base () 
        {
            Request = new ReviewApprenticehipUpdatesRequest { ApprenticeshipId = 1, AccountId = 1 };
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> ReviewApprenticeshipUpdates()
        {
            return await _controller.ReviewApprenticeshipUpdates(Request);
        }     


        internal void VerifyViewModelMapperIsCalled()
        {
            _mockMapper.Verify(x => x.Map<ReviewApprenticeshipUpdatesRequestViewModel>(It.IsAny<ReviewApprenticehipUpdatesRequest>()), Times.Once());
        }

        internal void VerifyViewResultIsReturned(IActionResult result)
        {
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
