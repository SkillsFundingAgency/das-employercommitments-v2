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
    public class WhenPostingResumeRequestConfirmation : ApprenticeControllerTestBase
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
        public async Task AndConfirmResumeIsSelected_ThenCommitmentsApiResumeApprenticeshipIsCalled(ResumeRequestViewModel request)
        {
            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            await _controller.ResumeApprenticeship(request);

            _mockCommitmentsApiClient.Verify(p => 
                p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmResumeIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
        {
            request.ResumeConfirmed = true;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.ResumeApprenticeship(request) as RedirectResult;

            Assert.AreEqual(_apprenticeshipDetailsUrl, result.Url);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenCommitmentsApiResumeApprenticeshipIsNotCalled(ResumeRequestViewModel request)
        {
            request.ResumeConfirmed = false;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            await _controller.ResumeApprenticeship(request);

            _mockCommitmentsApiClient.Verify(p => p.ResumeApprenticeship(It.IsAny<ResumeApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndGoBackIsSelected_ThenRedirectToApprenticeDetailsPage(ResumeRequestViewModel request)
        {
            request.ResumeConfirmed = false;

            _mockLinkGenerator.Setup(p => p.CommitmentsLink($"accounts/{request.AccountHashedId}/apprentices/manage/{request.ApprenticeshipHashedId}/details"))
                .Returns(_apprenticeshipDetailsUrl);

            var result = await _controller.ResumeApprenticeship(request) as RedirectResult;

            Assert.AreEqual(_apprenticeshipDetailsUrl, result.Url);
        }
    }
}
