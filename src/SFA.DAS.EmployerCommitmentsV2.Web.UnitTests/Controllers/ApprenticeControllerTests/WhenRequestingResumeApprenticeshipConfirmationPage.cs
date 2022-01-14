using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenRequestingResumeApprenticeshipConfirmationPage : ApprenticeControllerTestBase
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
        public async Task AndCurrentStatusIsPaused_ThenViewIsReturned()
        {
            DateTime resumedDate = DateTime.Now;
            _mockModelMapper.Setup(m => m.Map<ResumeRequestViewModel>(It.IsAny<ResumeRequest>()))
                .ReturnsAsync(new ResumeRequestViewModel()
                {
                    ResumeDate = resumedDate
                });

            var result = await _controller.ResumeApprenticeship(new ResumeRequest());

            var viewResult = (ViewResult)result;
            var viewModel = viewResult.Model;
            Assert.That(viewModel, Is.TypeOf<ResumeRequestViewModel>());
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
