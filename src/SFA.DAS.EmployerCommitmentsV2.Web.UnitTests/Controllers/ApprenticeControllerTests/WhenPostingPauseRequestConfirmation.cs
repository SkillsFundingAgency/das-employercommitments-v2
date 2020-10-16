using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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

            _controller = new ApprenticeController(_mockModelMapper.Object, _mockCookieStorageService.Object, _mockCommitmentsApiClient.Object, _mockLinkGenerator.Object);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmPauseIsSelected_ThenCommitmentsApiPauseApprenticeshipIsCalled(PauseRequestViewModel request)
        {
            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.PauseApprenticeship(request);

            _mockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmPauseIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
        {
            request.PauseConfirmed = true;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.PauseApprenticeship(request) as RedirectResult;

            Assert.AreEqual(_apprenticeshipDetailsUrl, result.Url);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenCommitmentsApiPauseApprenticeshipIsNotCalled(PauseRequestViewModel request)
        {
            request.PauseConfirmed = false;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.PauseApprenticeship(request);

            _mockCommitmentsApiClient.Verify(p => p.PauseApprenticeship(It.IsAny<PauseApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(PauseRequestViewModel request)
        {
            request.PauseConfirmed = false;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.PauseApprenticeship(request) as RedirectResult;

            Assert.AreEqual(_apprenticeshipDetailsUrl, result.Url);
        }
    }
}
