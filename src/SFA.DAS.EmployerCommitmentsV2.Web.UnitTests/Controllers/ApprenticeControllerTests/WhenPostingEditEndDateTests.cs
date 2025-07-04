using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using EditEndDateRequest = SFA.DAS.CommitmentsV2.Api.Types.Requests.EditEndDateRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingEditEndDateTests : ApprenticeControllerTestBase
{
    private const string ApprenticeEndDateUpdatedOnCompletedRecord = "New planned training finish date confirmed";
    private const string FlashMessageBody =nameof(FlashMessageBody);
    private const string FlashMessageLevel = nameof(FlashMessageLevel);
    private const string FlashMessageTitle = nameof(FlashMessageTitle);
    
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        CacheStorageService = new Mock<Interfaces.ICacheStorageService>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);
        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmEditDateIsSelected_ThenCommitmentsApiUpdateEndDateOfCompletedRecordIsCalled(EditEndDateViewModel request)
    {
        //Act
        await Controller.EditEndDate(request);

        //Assert
        MockCommitmentsApiClient.Verify(p => p.UpdateEndDateOfCompletedRecord(It.IsAny<EditEndDateRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task AndConfirmEditDateIsSelected_ThenRedirectToApprenticeDetailsPage(EditEndDateViewModel request)
    {
        //Act
        var result = await Controller.EditEndDate(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
    }

    [Test, MoqAutoData]
    public async Task AndConfirmEditDateIsSelected_ThenRedirectToApprenticeDetailsPageWithFlashMessage(EditEndDateViewModel request)
    {
        //Act
        var result = await Controller.EditEndDate(request) as RedirectToActionResult;

        Assert.Multiple(() =>
        {
            //Assert
            Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));
            Assert.That(Controller.TempData.Values.Contains(ApprenticeEndDateUpdatedOnCompletedRecord), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageBody), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageLevel), Is.True);
            Assert.That(Controller.TempData.ContainsKey(FlashMessageTitle), Is.True);
        });
    }
}