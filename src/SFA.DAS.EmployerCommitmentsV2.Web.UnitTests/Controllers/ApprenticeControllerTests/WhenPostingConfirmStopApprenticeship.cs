using AutoFixture;
using Microsoft.AspNetCore.Mvc;
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
    public class WhenPostingConfirmStopApprenticeship : ApprenticeControllerTestBase
    {
       
        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockLinkGenerator = new Mock<ILinkGenerator>();
            _mockModelMapper = new Mock<IModelMapper>();
            var stopApprenticeshipRequest = fixture.Create<StopApprenticeshipRequest>();

            _mockModelMapper.Setup(m => m.Map<StopApprenticeshipRequest>(It.IsAny<ConfirmStopRequestViewModel>()))
              .ReturnsAsync(stopApprenticeshipRequest);

            _controller = new ApprenticeController(_mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                _mockLinkGenerator.Object, 
                Mock.Of<ILogger<ApprenticeController>>(),
                Mock.Of<IAuthorizationService>());
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsStopped_AndIsCoPJourney_ThenRedirectToEnterNewTrainingProvider(ConfirmStopRequestViewModel viewModel)
        {
            var result = await _controller.ConfirmStop(viewModel);

            var redirect =  result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual(redirect.ActionName, "EnterNewTrainingProvider");
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
    }
}
