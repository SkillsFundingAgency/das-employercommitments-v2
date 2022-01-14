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
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingConfirmStopApprenticeship : ApprenticeControllerTestBase
    {
        private const string ApprenticeStoppedMessage = "Apprenticeship stopped";
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageTitle = "FlashMessageTitle";
        private const string FlashMessageLevel = "FlashMessageLevel";

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockModelMapper = new Mock<IModelMapper>();
            var stopApprenticeshipRequest = fixture.Create<StopApprenticeshipRequest>();

            _mockModelMapper.Setup(m => m.Map<StopApprenticeshipRequest>(It.IsAny<ConfirmStopRequestViewModel>()))
              .ReturnsAsync(stopApprenticeshipRequest);

            _controller = new ApprenticeController(_mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsStopped_AndIsCoPJourney_ThenRedirectToEnterNewTrainingProvider(ConfirmStopRequestViewModel viewModel)
        {
            var result = await _controller.ConfirmStop(viewModel);

            var redirect =  result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "ApprenticeshipStoppedInform");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsStopped_CommitmentApi_IsCalled(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = true;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(x => x.StopApprenticeship(viewModel.ApprenticeshipId, It.IsAny<StopApprenticeshipRequest>(), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsNotStopped_CommitmentApi_IsNotCalled(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = false;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(x => x.StopApprenticeship(It.IsAny<long>(), It.IsAny<StopApprenticeshipRequest>(), CancellationToken.None), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsStopped_and_Mapper_IsCalled(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = true;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            _mockModelMapper.Verify(x => x.Map<StopApprenticeshipRequest>(viewModel), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsNotStopped__and_Mapper_IsNotCalled(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = false;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            _mockModelMapper.Verify(x => x.Map<StopApprenticeshipRequest>(viewModel), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsNotStopped_And_IsCopJourney_ThenRedirectsToApprenticeshipDetails(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = false;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsNotStopped_And_IsNotCopJourney_ThenRedirectsToApprenticeshipDetails(ConfirmStopRequestViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = false;

            //Act
            var result = await _controller.ConfirmStop(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsStopped_ThenRedirectToApprenticeDetailsPageWithFlashMessage(ConfirmStopRequestViewModel request)
        {
            //Arrange
            request.StopConfirmed = true;
            request.IsCoPJourney = false;

            //Act
            var result = await _controller.ConfirmStop(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageBody));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageTitle));
        }

        [Test, MoqAutoData]
        public async Task AndApprenticeshipIsStoppedDuringChangeOfProvider_ThenSetStoppedDuringChangeOfProvider_IsTrue(ConfirmStopRequestViewModel viewModel)
        {
            viewModel.StopConfirmed = true;
            viewModel.IsCoPJourney = true;
            var result = await _controller.ConfirmStop(viewModel);

            var redirectResult = (RedirectToActionResult)result;
            var routeValues = redirectResult.RouteValues;

            Assert.AreEqual(true, routeValues["StoppedDuringCoP"]);
            Assert.IsFalse(_controller.TempData.Values.Contains(ApprenticeStoppedMessage));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessageBody));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessageLevel));
            Assert.IsFalse(_controller.TempData.ContainsKey(FlashMessageTitle));
        }


    }
}
