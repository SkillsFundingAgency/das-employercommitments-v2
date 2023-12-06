using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingConfirmEditApprenticeshipTests : ApprenticeControllerTestBase
    {
        private const string EditApprenticeUpdated = "You have updated apprentice details";
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageLevel = "FlashMessageLevel";
        private EditApprenticeshipResponse response;

        [SetUp]
        public void Arrange()
        {
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockModelMapper = new Mock<IModelMapper>();
            response = new EditApprenticeshipResponse { NeedReapproval = true };

            _mockCommitmentsApiClient.Setup(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(response));

            _controller = new ApprenticeController(_mockModelMapper.Object,
                Mock.Of<ICookieStorageService<IndexRequest>>(),
                _mockCommitmentsApiClient.Object,
                Mock.Of<ILogger<ApprenticeController>>());
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsConfirmed_WithIntermediateUpdate_ThenRedirectToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
        {
            viewModel.ConfirmChanges = true;
            var result = await _controller.ConfirmEditApprenticeship(viewModel);

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.That("ApprenticeshipDetails", Is.EqualTo(redirect.ActionName));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));

            Assert.That(_controller.TempData.Values.Contains($"Your suggested changes have been sent to {viewModel.ProviderName} for approval."), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
        }

        [Test, MoqAutoData]
        public async Task AndTheApprenticeship_IsEdit_WithImmediateUpdate_ThenRedirectToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
        {
            response.NeedReapproval = false;
            var result = await _controller.ConfirmEditApprenticeship(viewModel);

            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.That("ApprenticeshipDetails", Is.EqualTo(redirect.ActionName));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));

            Assert.That(_controller.TempData.Values.Contains(EditApprenticeUpdated), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(_controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsConfirmed_CommitmentApi_IsCalled(ConfirmEditApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.ConfirmChanges = true;

            //Act
            var result = await _controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            _mockCommitmentsApiClient.Verify(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsNotConfirmed_CommitmentApi_IsNotCalled(ConfirmEditApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.ConfirmChanges = false;

            //Act
            var result = await _controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;


            //Assert
            _mockCommitmentsApiClient.Verify(x => x.EditApprenticeship(It.IsAny<EditApprenticeshipApiRequest>(), CancellationToken.None), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsConfirmed_and_Mapper_IsCalled(ConfirmEditApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.ConfirmChanges = true;

            //Act
            var result = await _controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            _mockModelMapper.Verify(x => x.Map<EditApprenticeshipApiRequest>(viewModel), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsNotConfirmed__and_Mapper_IsNotCalled(ConfirmEditApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.ConfirmChanges = false;

            //Act
            var result = await _controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            _mockModelMapper.Verify(x => x.Map<EditApprenticeshipApiRequest>(viewModel), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task AndTheEditApprenticeship_IsNotConfirmed_ThenRedirectsToApprenticeshipDetails(ConfirmEditApprenticeshipViewModel viewModel)
        {
            //Arrange
            viewModel.ConfirmChanges = false;

            //Act
            var result = await _controller.ConfirmEditApprenticeship(viewModel) as RedirectToActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
        }
    }
}
