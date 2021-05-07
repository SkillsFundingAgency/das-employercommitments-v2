using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingConfirmEditApprenticeshipTests
    {
        private WhenRequestingConfirmEditApprenticeshipFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenRequestingConfirmEditApprenticeshipFixture();
        }   

        [Test]
        public async Task VerifyViewModelMapperIsCalled()
        {
             await _fixture.ConfirmEditApprenticeship();
            _fixture.VerifyViewModelMapperIsCalled();
        }

        [Test]
        public async Task VerifyViewIsReturned()
        {
           var result =  await _fixture.ConfirmEditApprenticeship();
            _fixture.VerifyViewResultIsReturned(result);
        }
    }

    public class WhenRequestingConfirmEditApprenticeshipFixture : ApprenticeControllerTestFixtureBase
    {
        public WhenRequestingConfirmEditApprenticeshipFixture() : base () 
        {
            _controller.TempData = new TempDataDictionary( Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
        }

        public async Task<IActionResult> ConfirmEditApprenticeship()
        {
            return await _controller.ConfirmEditApprenticeship();
        }     

        public void VerifyValidationApiIsCalled()
        {
            _mockCommitmentsApiClient.Verify(x => x.ValidateApprenticeshipForEdit(It.IsAny<ValidateApprenticeshipForEditRequest>(), CancellationToken.None), Times.Once());
        }

        internal void VerifyViewModelMapperIsCalled()
        {
            _mockMapper.Verify(x => x.Map<ConfirmEditApprenticeshipViewModel>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
        }

        internal void VerifyViewResultIsReturned(IActionResult result)
        {
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
