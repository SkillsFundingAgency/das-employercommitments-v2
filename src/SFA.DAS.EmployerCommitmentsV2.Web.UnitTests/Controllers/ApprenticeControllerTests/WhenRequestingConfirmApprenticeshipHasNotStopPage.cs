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
    public class WhenRequestingConfirmApprenticeshipHasNotStopPage : ApprenticeControllerTestBase
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
        public async Task WhenRequesting_ConfirmApprenticeshipHasNotStop_ThenConfirmHasNotStopRequestViewModelIsPassedToTheView(ConfirmHasNotStopViewModel expectedViewModel)
        {
            _mockModelMapper
                .Setup(m => m.Map<ConfirmHasNotStopViewModel>(It.IsAny<ConfirmHasNotStopRequest>()))
                .ReturnsAsync(expectedViewModel);

            var viewResult = await _controller.ConfirmHasNotStop(new ConfirmHasNotStopRequest()) as ViewResult;
            var viewModel = viewResult.Model;

            var actualViewModel = (ConfirmHasNotStopViewModel)viewModel;

            Assert.That(viewModel, Is.InstanceOf<ConfirmHasNotStopViewModel>());
            Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
        }
    }
}