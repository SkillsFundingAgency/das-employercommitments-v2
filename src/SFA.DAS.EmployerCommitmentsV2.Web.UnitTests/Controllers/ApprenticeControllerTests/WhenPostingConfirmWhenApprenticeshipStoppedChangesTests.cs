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
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingConfirmWhenApprenticeshipStoppedChangesTests : ApprenticeControllerTestBase
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
        public async Task AndTheApprenticeship_StoppedIsWrong_ThenRedirectToApprenticeshipDetails(ConfirmWhenApprenticeshipStoppedViewModel viewModel)
        {
            viewModel.IsCorrectStopDate = false;

            var result = await _controller.ConfirmWhenApprenticeshipStopped(viewModel) as RedirectToActionResult;

            var redirect = result.VerifyReturnsRedirectToActionResult();

            Assert.AreEqual(redirect.ActionName, "EditStopDate");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);

            _mockCommitmentsApiClient.Verify(x => x.ResolveOverlappingTrainingDateRequest(It.IsAny<ResolveApprenticeshipOverlappingTrainingDateRequest>(), CancellationToken.None), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_StoppedIsCorrect_ThenRedirectToApprenticeshipDetails(ConfirmWhenApprenticeshipStoppedViewModel viewModel)
        {
            //Arrange
            viewModel.IsCorrectStopDate = true;
            viewModel.StopDate = new DateTime(2022, 1, 1);

            //Act
            var result = await _controller.ConfirmWhenApprenticeshipStopped(viewModel) as RedirectToActionResult;

            //Assert
            var redirect = result.VerifyReturnsRedirectToActionResult();

            Assert.IsTrue(_controller.TempData.Values.Contains("Current stop date confirmed"));
            Assert.IsTrue(_controller.TempData.Values.Contains("January 2022"));
            Assert.AreEqual(redirect.ActionName, "ApprenticeshipDetails");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);

            _mockCommitmentsApiClient
                .Verify(x => x.ResolveOverlappingTrainingDateRequest(It.Is<ResolveApprenticeshipOverlappingTrainingDateRequest>(x => x.ResolutionType == OverlappingTrainingDateRequestResolutionType.ApprenticeshipStopDateIsCorrect), CancellationToken.None), Times.Once);
        }
    }
}