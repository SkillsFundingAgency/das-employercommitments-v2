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
    public class WhenRequestingReconfirmHasNotStopPage : ApprenticeControllerTestBase
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
        public async Task WhenRequesting_ReconfirmApprenticeshipHasNotStop_ThenReConfirmHasNotStopRequestViewModelIsPassedToTheView(ReconfirmHasNotStopViewModel expectedViewModel)
        {
            _mockModelMapper
                .Setup(m => m.Map<ReconfirmHasNotStopViewModel>(It.IsAny<ReConfirmHasNotStopRequest>()))
                .ReturnsAsync(expectedViewModel);

            var viewResult = await _controller.ReconfirmHasNotStop(new ReConfirmHasNotStopRequest()) as ViewResult;
            var viewModel = viewResult.Model;

            var actualViewModel = (ReconfirmHasNotStopViewModel)viewModel;

            Assert.IsInstanceOf<ReconfirmHasNotStopViewModel>(viewModel);
            Assert.AreEqual(expectedViewModel, actualViewModel);
        }
    }
}