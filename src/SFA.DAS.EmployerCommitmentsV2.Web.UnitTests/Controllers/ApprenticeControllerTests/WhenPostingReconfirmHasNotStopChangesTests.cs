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
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingReconfirmHasNotStopChangesTests : ApprenticeControllerTestBase
    {
        private string ApprenticeHasNotStoppedConrimedMessage;
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageTitle = "FlashMessageTitle";
        private const string FlashMessageLevel = "FlashMessageLevel";

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
        public async Task AndTheApprenticeship_IsNotStoppedIsConfirmed_ThenRedirectToApprenticeshipDetails(ReconfirmHasNotStopViewModel viewModel)
        {
            var result = await _controller.ReconfirmHasNotStopChangesAsync(viewModel);

            var redirect = result.VerifyReturnsRedirectToActionResult();

            Assert.IsTrue(_controller.TempData.Values.Contains($"Apprenticeship confirmed for {viewModel.ULN}"));
            Assert.AreEqual(redirect.ActionName, "ApprenticeshipDetails");
            Assert.AreEqual(redirect.RouteValues["AccountHashedId"], viewModel.AccountHashedId);
            Assert.AreEqual(redirect.RouteValues["ApprenticeshipHashedId"], viewModel.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsNotStoppedIsConfirmed_CommitmentApi_IsCalled(ReconfirmHasNotStopViewModel viewModel)
        {
            //Arrange
            viewModel.StopConfirmed = true;

            //Act
            var result = await _controller.ReconfirmHasNotStopChangesAsync(viewModel) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(x => x.ResolveOverlappingTrainingDateRequest(It.IsAny<ResolveApprenticeshipOverlappingTrainingDateRequest>(), CancellationToken.None), Times.Once);
        }
    }
}