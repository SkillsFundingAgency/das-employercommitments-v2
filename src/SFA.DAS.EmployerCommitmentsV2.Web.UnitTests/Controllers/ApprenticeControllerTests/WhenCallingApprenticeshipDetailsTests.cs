using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Employer.Shared.UI.Models.Flags;

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
        [TestCase(ApprenticeshipStatus.Live)]
        [TestCase(ApprenticeshipStatus.Live, ApprenticeDetailsBanners.ChangeOfPriceApproved)]
        [TestCase(ApprenticeshipStatus.Live, ApprenticeDetailsBanners.ChangeOfPriceApproved | ApprenticeDetailsBanners.ChangeOfPriceCancelled)]
        public async Task ThenTheCorrectViewIsReturned(ApprenticeshipStatus apprenticeshipStatus, ApprenticeDetailsBanners banners = 0)
        {
            _fixture = new WhenCallingApprenticeshipDetailsTestsFixture(apprenticeshipStatus);

            var result = await _fixture.ApprenticeshipDetails(banners);

            _fixture.VerifyViewModel(result as ViewResult);
            _fixture.VerifyBanners(result as ViewResult, banners);
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

        public WhenCallingApprenticeshipDetailsTestsFixture(ApprenticeshipStatus apprenticeshipStatus) : base()
        {
            _request = AutoFixture.Create<ApprenticeshipDetailsRequest>();
            _viewModel = AutoFixture.Create<ApprenticeshipDetailsRequestViewModel>();
            _viewModel.ApprenticeshipStatus = apprenticeshipStatus;
            Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);

            MockMapper.Setup(m => m.Map<ApprenticeshipDetailsRequestViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ApprenticeshipDetails(ApprenticeDetailsBanners banners = 0)
        {
            return await Controller.ApprenticeshipDetails(_request, banners);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.That(_viewModel, Is.EqualTo(viewModel));
        }

        public void VerifyNoFlashMessage(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.That(_viewModel, Is.EqualTo(viewModel));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeStoppedMessage), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessage), Is.False);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.False);
        }

        public void VerifyBanners(ViewResult viewResult, ApprenticeDetailsBanners expectedBanners)
        {
            var viewModel = viewResult.Model as ApprenticeshipDetailsRequestViewModel;

            Assert.IsInstanceOf<ApprenticeshipDetailsRequestViewModel>(viewModel);
            Assert.That(_viewModel.ShowBannersFlags, Is.EqualTo(expectedBanners));
        }
    }
}