using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingConfirmHasValidEndDateChanges : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
        MockModelMapper = new Mock<IModelMapper>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            Mock.Of<ICookieStorageService<IndexRequest>>(),
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());

        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_HasValidEndDate_ThenRedirectToApprenticeshipDetails(ConfirmHasValidEndDateViewModel request)
    {
        request.EndDateConfirmed = true;

        var result = await Controller.ConfirmHasValidEndDateChanges(request) as RedirectToActionResult;

        Assert.That(result.ActionName, Is.EqualTo("ApprenticeshipDetails"));

    }

    [Test, MoqAutoData]
    public async Task AndTheApprenticeship_HasNotValidEndDate_ThenRedirectToEditEndDateApprenticeship (ConfirmHasValidEndDateViewModel request)
    {
        request.EndDateConfirmed = false;

        var result = await Controller.ConfirmHasValidEndDateChanges(request) as RedirectToActionResult;

        Assert.That(result.ActionName, Is.EqualTo("EditEndDate"));
    }
}