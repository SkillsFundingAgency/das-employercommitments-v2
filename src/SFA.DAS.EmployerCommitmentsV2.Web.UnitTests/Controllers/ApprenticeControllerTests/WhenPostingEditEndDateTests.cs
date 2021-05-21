using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using EditEndDateRequest = SFA.DAS.CommitmentsV2.Api.Types.Requests.EditEndDateRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingEditEndDateTests : ApprenticeControllerTestBase
    {
        private const string ApprenticeEndDateUpdatedOnCompletedRecord = "New planned training finish date confirmed";
        private const string FlashMessage = "FlashMessage";
        private const string FlashMessageLevel = "FlashMessageLevel";

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
            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmEditDateIsSelected_ThenCommitmentsApiUpdateEndDateOfCompletedRecordIsCalled(EditEndDateViewModel request)
        {  
            //Act
            var result = await _controller.EditEndDate(request) as RedirectToActionResult;

           //Assert
            _mockCommitmentsApiClient.Verify(p => p.UpdateEndDateOfCompletedRecord(It.IsAny<EditEndDateRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmEditDateIsSelected_ThenRedirectToApprenticeDetailsPage(EditEndDateViewModel request)
        {
            //Act
            var result = await _controller.EditEndDate(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
        }

        [Test, MoqAutoData]
        public async Task AndConfirmEditDateIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(EditEndDateViewModel request)
        {
            //Act
            var result = await _controller.EditEndDate(request) as RedirectToActionResult;

            //Assert
            Assert.AreEqual("ApprenticeshipDetails", result.ActionName);
            Assert.IsTrue(_controller.TempData.Values.Contains(ApprenticeEndDateUpdatedOnCompletedRecord));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessage));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
        }
    }
}
