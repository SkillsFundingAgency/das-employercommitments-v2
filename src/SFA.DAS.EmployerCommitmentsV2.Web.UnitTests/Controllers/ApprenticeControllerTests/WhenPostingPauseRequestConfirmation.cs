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
    public class WhenPostingPauseRequestConfirmation : ApprenticeControllerTestBase
    {
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
        }

        [Test, MoqAutoData]
        public async Task AndConfirmPauseIsSelected_ThenCommitmentsApiPauseApprenticeshipIsCalled(PauseRequestViewModel request)
        {  
            //Act
            var result = await _controller.PauseApprenticeship(request) as RedirectToActionResult;

           //Assert
            _mockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmPauseIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
        {
            //Arrange
            request.PauseConfirmed = true;

            //Act
            var result = await _controller.PauseApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenCommitmentsApiPauseApprenticeshipIsNotCalled(PauseRequestViewModel request)
        {
            //Arrange
            request.PauseConfirmed = false;

            //Act
            var result = await _controller.PauseApprenticeship(request) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
        {
            //Arrange
            request.PauseConfirmed = false;

            //Act
            var result = await _controller.PauseApprenticeship(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }
    }
}
