using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingChangeStatusPage : ApprenticeControllerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            _mockModelMapper = new Mock<IModelMapper>();
            _mockCookieStorageService = new Mock<ICookieStorageService<IndexRequest>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockLinkGenerator = new Mock<ILinkGenerator>();

            _controller = new ApprenticeController(_mockModelMapper.Object, _mockCookieStorageService.Object, _mockCommitmentsApiClient.Object, _mockLinkGenerator.Object);
        }

        [Test]
        public async Task AndCurrentStatusIsLive_ThenViewIsReturned()
        {
            _mockModelMapper.Setup(m => m.Map<ChangeStatusRequestViewModel>(It.IsAny<ChangeStatusRequest>()))
                .ReturnsAsync(new ChangeStatusRequestViewModel { CurrentStatus = ApprenticeshipStatus.Live });

            var result = await _controller.ChangeStatus(new ChangeStatusRequest());

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
