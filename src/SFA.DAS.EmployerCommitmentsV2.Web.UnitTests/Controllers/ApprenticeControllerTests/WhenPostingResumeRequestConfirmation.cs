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
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingResumeRequestConfirmation : ApprenticeControllerTestBase
    {
        private const string ApprenticeResumeMessage = "Apprenticeship resumed";
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageLevel = "FlashMessageLevel";
        private const string FlashMessageTitle = "FlashMessageTitle";

        [SetUp]
        public void Arrange()
        {
            _mockModelMapper = new Mock<IModelMapper>();
            _mockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _controller = new ApprenticeController(_mockModelMapper.Object,
                _mockCookieStorageService.Object,
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmResumeIsSelected_ThenCommitmentsApiResumeApprenticeshipIsCalled(ResumeRequestViewModel request)
        {
            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(p => 
                p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = true;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
        }

        [Test, MoqAutoData]
        public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = true;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(_controller.TempData.Values.Contains(ApprenticeResumeMessage), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenCommitmentsApiResumeApprenticeshipIsNotCalled(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = false;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(p => p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = false;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPageWithoutFlashMessage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = false;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(_controller.TempData.Values.Contains(ApprenticeResumeMessage), Is.False);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageBody), Is.False);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageLevel), Is.False);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageTitle), Is.False);
        }
    }
}
