using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingPauseApprenticeshipConfirmationPage : ApprenticeControllerTestBase
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

        [Test]
        public async Task AndCurrentStatusIsLive_ThenViewIsReturned()
        {
            _mockModelMapper.Setup(m => m.Map<PauseRequestViewModel>(It.IsAny<PauseRequest>()))
                .ReturnsAsync(new PauseRequestViewModel());

            var result = await _controller.PauseApprenticeship(new PauseRequest());

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test, MoqAutoData]
        public async Task AndCurrentStatusIsLive_ThenPauseRequestViewModelIsPassedToTheView(PauseRequestViewModel _viewModel)
        {
            _mockModelMapper.Setup(m => m.Map<PauseRequestViewModel>(It.IsAny<PauseRequest>()))
                .ReturnsAsync(_viewModel);

            var viewResult = await _controller.PauseApprenticeship(new PauseRequest()) as ViewResult;
            var viewModel = viewResult.Model;

            var pauseRequestViewModel = (PauseRequestViewModel)viewModel;

            Assert.That(viewModel, Is.InstanceOf<PauseRequestViewModel>());
            Assert.That(pauseRequestViewModel, Is.EqualTo(_viewModel));
        }
    }
}
