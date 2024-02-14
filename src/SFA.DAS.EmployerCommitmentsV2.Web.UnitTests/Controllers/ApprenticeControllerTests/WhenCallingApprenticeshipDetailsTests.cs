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

        [TestCase(ApprenticeshipStatus.Live, false, false)]
        [TestCase(ApprenticeshipStatus.Paused, false, false)]
        [TestCase(ApprenticeshipStatus.WaitingToStart, false, false)]
        [TestCase(ApprenticeshipStatus.Stopped, false, false)]
        [TestCase(ApprenticeshipStatus.Completed, false, false)]
        [TestCase(ApprenticeshipStatus.Live, true, false)]
        [TestCase(ApprenticeshipStatus.Live, false, true)]
        public async Task ThenTheCorrectViewIsReturned(ApprenticeshipStatus apprenticeshipStatus, bool showPriceChangeRejected, bool showPriceChangeApproved)
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(apprenticeshipStatus);

            var result = await _fixture.ApprenticeshipDetails();

            _fixture.VerifyViewModel(result as ViewResult);
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturnedWithoutFlashMessage()
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus.Stopped);

            var result = await _fixture.ApprenticeshipDetails();

            _fixture.VerifyNoFlashMessage(result as ViewResult);
        }
    }

    public class WhenCallingApprenticeshipDetailsTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly ApprenticeshipDetailsRequest _request;
        private readonly ApprenticeshipDetailsRequestViewModel _viewModel;
        private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
        private const string FlashMessage = "FlashMessage";
        private const string FlashMessageLevel = "FlashMessageLevel";

        public WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus apprenticeshipStatus, bool showPriceChangeRejected = false, bool showPriceChangeApproved = false) : base()
        {
            _request = _autoFixture.Create<ApprenticeshipDetailsRequest>();
            _viewModel = _autoFixture.Create<ApprenticeshipDetailsRequestViewModel>();
            _viewModel.ApprenticeshipStatus = apprenticeshipStatus;
            _viewModel.ShowPriceChangeRejected = showPriceChangeRejected;
            _viewModel.ShowPriceChangeApproved = showPriceChangeApproved;
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

        public void VerifyNoFlashMessage(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;
         
            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
            Assert.IsFalse(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessageLevel));
        }
    }
}
