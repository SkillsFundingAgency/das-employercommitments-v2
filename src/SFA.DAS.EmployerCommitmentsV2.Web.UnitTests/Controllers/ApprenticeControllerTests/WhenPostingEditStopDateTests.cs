using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingEditStopDateTests : ApprenticeControllerTestBase
    {
        private const string ApprenticeEditStopDate = "New stop date confirmed";
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageLevel = "FlashMessageLevel";
        private const string FlashMessageTitle = "FlashMessageTitle";
        private Fixture _autoFixture;
        private EditStopDateViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {           
            _mockModelMapper = new Mock<IModelMapper>();
            _mockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _autoFixture = new Fixture();
            _autoFixture.Customize<EditStopDateViewModel>(c => c.Without(x => x.NewStopDate));
            _viewModel = _autoFixture.Create<EditStopDateViewModel>();
            _viewModel.NewStopDate = new CommitmentsV2.Shared.Models.MonthYearModel("062020");

            _controller = new ApprenticeController(_mockModelMapper.Object,
                _mockCookieStorageService.Object,
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test]
        public async Task AndSubmit_ThenCommitmentsApiUpdateApprenticeshipStopDateIsCalled()
        {
            //Act
            var result = await _controller.UpdateApprenticeshipStopDate(_viewModel) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(p =>
                p.UpdateApprenticeshipStopDate(It.IsAny<long>(), It.IsAny<ApprenticeshipStopDateRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task AndAfterUpdateDate_ThenRedirectToApprenticeDetailsPage()
        {
            //Act
            var result = await _controller.UpdateApprenticeshipStopDate(_viewModel) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
        }

        [Test]
        public async Task AndAfterUpdateDate_ThenRedirectToApprenticeDetailsPageWithFlashMessage()
        {
            //Act
            var result = await _controller.UpdateApprenticeshipStopDate(_viewModel) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(_controller.TempData.Values.Contains(ApprenticeEditStopDate), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        }
    }
}
