using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingChangeStatus : ApprenticeControllerTestBase
    {
        private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
        private const string ApprenticeResumeMessage = "Apprenticeship resumed";
        public const string ApprenticePausedMessage = "Apprenticeship paused";
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
        public void AndPauseIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = ChangeStatusType.Pause;
           
            //Act
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("PauseApprenticeship", response.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticePausedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }

        [Test, MoqAutoData]
        public void AndGoBackIsSelected_ThenRedirectToPauseApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = ChangeStatusType.GoBack;

            //Act
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndStopIsSelected_ThenRedirectToV1WhenToApplyStopAction(ChangeStatusRequestViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = ChangeStatusType.Stop;
            viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.Live;

            //Act
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }

        [Test, MoqAutoData]
        public void AndStopIsSelected_ThenRedirectToV1WhenToHasTheApprenticeBeenMadeRedundant(ChangeStatusRequestViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = ChangeStatusType.Stop;
            viewModel.CurrentStatus = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;

            //Act
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("HasTheApprenticeBeenMadeRedundant", response.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }

        [Test, MoqAutoData]
        public void AndPauseIsSelected_ThenRedirectToResumeApprenticeshipAction(ChangeStatusRequestViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = ChangeStatusType.Resume;

            //Act
            var response = _controller.ChangeStatus(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ResumeApprenticeship", response.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeResumeMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }
    }
}
