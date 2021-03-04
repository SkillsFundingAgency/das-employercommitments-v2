using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingResumeRequestConfirmation : ApprenticeControllerTestBase
    {
        private const string ApprenticeResumeMessage = "Apprenticeship resumed";
        private const string FlashMessage = "FlashMessage";
        private const string FlashMessageLevel = "FlashMessageLevel";

        [SetUp]
        public void Arrange()
        {
            _mockModelMapper = new Mock<IModelMapper>();
            _mockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockLinkGenerator = new Mock<ILinkGenerator>();

            _controller = new ApprenticeController(_mockModelMapper.Object, 
                _mockCookieStorageService.Object, 
                _mockCommitmentsApiClient.Object, 
                _mockLinkGenerator.Object, 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
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
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = true;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeResumeMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
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
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPageWithoutFlashMessage(ResumeRequestViewModel request)
        {
            //Arrange
            request.ResumeConfirmed = false;

            //Act
            var result = await _controller.ResumeApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
            Assert.IsFalse(_controller.TempData.Values.Contains(ApprenticeResumeMessage));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessageLevel));
        }
    }
}
