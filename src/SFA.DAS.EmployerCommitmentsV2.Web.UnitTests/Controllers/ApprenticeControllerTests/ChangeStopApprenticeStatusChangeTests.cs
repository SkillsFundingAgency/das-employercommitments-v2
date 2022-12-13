using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.WhyStopApprenticeshipViewModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class ChangeStopApprenticeStatusChangeTests : ApprenticeControllerTestBase
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
        public void AndLeftEmploymentIsSelected_ThenRedirectToStopApprenticeshipAction(WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.LeftEmployment;

            //Act
            var response = _controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndChangeProviderIsSelected_ThenRedirectToStopApprenticeshipAction(WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.ChangeProvider;

            //Act
            var response = _controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndWithdrawnIsSelected_ThenRedirectToStopApprenticeshipAction(WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.Withdrawn;

            //Act
            var response = _controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("StopApprenticeship", response.ActionName);
        }

        [Test, MoqAutoData]
        public void AndNeverStartedIsSelected_ThenRedirectToStopApprenticeshipAction(WhyStopApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.SelectedStatusChange = StopStatusReason.NeverStarted;

            //Act
            var response = _controller.WhyStopApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipNeverStarted", response.ActionName);
        }
        
    }
}
