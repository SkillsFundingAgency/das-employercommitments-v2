using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingApprenticeshipDetailsTests
    {
        WhenCallingApprenticeshipDetailsTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus.Live);
        }

        [TestCase(ApprenticeshipStatus.Live)]
        [TestCase(ApprenticeshipStatus.Paused)]
        [TestCase(ApprenticeshipStatus.WaitingToStart)]
        [TestCase(ApprenticeshipStatus.Stopped)]
        [TestCase(ApprenticeshipStatus.Completed)]
        public async Task ThenTheCorrectViewIsReturned(ApprenticeshipStatus apprenticeshipStatus)
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(apprenticeshipStatus);

            var result = await _fixture.ApprenticeshipDetails();

            _fixture.VerifyViewModel(result as ViewResult);
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturnedWithFlashMessage()
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus.Stopped);

            var result = await _fixture.ApprenticeshipDetails();

            _fixture.VerifyFlashMessage(result as ViewResult);
        } 
    }

    public class WhenCallingApprenticeshipDetailsTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ApprenticeshipDetailsRequest _request;
        private readonly ApprenticeshipDetailsRequestViewModel _viewModel;
        private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
        private const string FlashMessage = "FlashMessage";
        private const string FlashMessageLevel = "FlashMessageLevel";

        public WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus apprenticeshipStatus) : base()
        {
            _request = _autoFixture.Create<ApprenticeshipDetailsRequest>();
            _viewModel = _autoFixture.Create<ApprenticeshipDetailsRequestViewModel>();
            _viewModel.ApprenticeshipStatus = apprenticeshipStatus;
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);

            _mockMapper.Setup(m => m.Map<ApprenticeshipDetailsRequestViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ApprenticeshipDetails()
        {
            return await _controller.ApprenticeshipDetails(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }

        public void VerifyFlashMessage(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;
         
            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }
    }
}
