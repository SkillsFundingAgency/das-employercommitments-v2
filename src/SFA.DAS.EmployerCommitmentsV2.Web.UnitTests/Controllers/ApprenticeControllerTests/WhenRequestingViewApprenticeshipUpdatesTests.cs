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
    public class WhenRequestingViewApprenticeshipUpdatesTests
    {
        private WhenRequestingViewApprenticeshipUpdatesTestsixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingViewApprenticeshipUpdatesTestsixture();
        }   

        [Test]
        public async Task VerifyViewModelMapperIsCalled()
        {
             await _fixture.ViewApprenticeshipUpdates();
            _fixture.VerifyViewModelMapperIsCalled();
        }

        [Test]
        public async Task VerifyViewIsReturned()
        {
           var result =  await _fixture.ViewApprenticeshipUpdates();
            _fixture.VerifyViewResultIsReturned(result);
        }
    }

    public class WhenRequestingViewApprenticeshipUpdatesTestsixture : ApprenticeControllerTestFixtureBase
    {
        public ViewApprenticehipUpdatesRequest Request { get; set; }
        public WhenRequestingViewApprenticeshipUpdatesTestsixture() : base () 
        {
            Request = new ViewApprenticehipUpdatesRequest { ApprenticeshipId = 1, AccountId = 1 };
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> ViewApprenticeshipUpdates()
        {
            return await _controller.ViewApprenticeshipUpdates(Request);
        }     

        internal void VerifyViewModelMapperIsCalled()
        {
            _mockMapper.Verify(x => x.Map<ViewApprenticeshipUpdatesRequestViewModel>(It.IsAny<ViewApprenticehipUpdatesRequest>()), Times.Once());
        }

        internal void VerifyViewResultIsReturned(IActionResult result)
        {
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
