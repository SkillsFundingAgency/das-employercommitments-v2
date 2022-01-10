using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using EditEndDateRequest = SFA.DAS.CommitmentsV2.Api.Types.Requests.EditEndDateRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenPostingEditEndDateTests : ApprenticeControllerTestBase
    {
        private const string ApprenticeEndDateUpdatedOnCompletedRecord = "New planned training finish date confirmed";
        private const string FlashMessageBody = "FlashMessageBody";
        private const string FlashMessageLevel = "FlashMessageLevel";
        private const string FlashMessageTitle = "FlashMessageTitle";

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
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageBody));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageLevel));
            Assert.IsTrue(_controller.TempData.ContainsKey(FlashMessageTitle));
        }
    }
}
