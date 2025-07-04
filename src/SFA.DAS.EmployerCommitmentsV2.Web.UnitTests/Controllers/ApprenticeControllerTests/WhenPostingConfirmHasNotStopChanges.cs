using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingConfirmHasNotStopChanges : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();
        CacheStorageService = new Mock<Interfaces.ICacheStorageService>();
        ApprovalsApiClient = new Mock<IApprovalsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            CacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            ApprovalsApiClient.Object);

        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public void AndTheApprenticeship_IsStopped_ThenRedirectToStopApprenticeship(ConfirmHasNotStopViewModel request)
    {
        //Arrange
        request.StopConfirmed = true;

        //Act
        var result = Controller.ConfirmHasNotStopChanges(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("StopApprenticeship"));
    }

    [Test, MoqAutoData]
    public void AndTheApprenticeship_IsNotStopped_ThenRedirectToReconfirmHasNotStop(ConfirmHasNotStopViewModel request)
    {
        //Arrange
        request.StopConfirmed = false;

        //Act
        var result = Controller.ConfirmHasNotStopChanges(request) as RedirectToActionResult;

        //Assert
        Assert.That(result.ActionName, Is.EqualTo("ReconfirmHasNotStop"));
    }
}