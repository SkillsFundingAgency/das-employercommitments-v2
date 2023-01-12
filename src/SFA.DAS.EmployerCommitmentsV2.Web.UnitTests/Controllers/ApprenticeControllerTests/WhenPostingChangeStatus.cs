using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenPostingChangeStatus : ApprenticeControllerTestBase
    {
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
            Assert.AreEqual("WhyStopApprenticeship", response.ActionName);           
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
        }
    }
}
