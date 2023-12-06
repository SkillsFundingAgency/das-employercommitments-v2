using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingConfirmHasNotStopChanges : ApprenticeControllerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockModelMapper = new Mock<IModelMapper>();

            _controller = new ApprenticeController(_mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public void AndTheApprenticeship_IsStopped_ThenRedirectToStopApprenticeship(ConfirmHasNotStopViewModel request)
        {
            //Arrange
            request.StopConfirmed = true;

            //Act
            var result = _controller.ConfirmHasNotStopChanges(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("StopApprenticeship"));
        }

        [Test, MoqAutoData]
        public void AndTheApprenticeship_IsNotStopped_ThenRedirectToReconfirmHasNotStop(ConfirmHasNotStopViewModel request)
        {
            //Arrange
            request.StopConfirmed = false;

            //Act
            var result = _controller.ConfirmHasNotStopChanges(request) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ReconfirmHasNotStop"));
        }
    }
}