using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingWhenDidThisApprenticeshipStop : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        Controller = new ApprenticeController(Mock.Of<IModelMapper>(),
            Mock.Of<ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test]
    public void AndStopDateIsEntered_ThenRedirectToHasApprenticeBeenMadeRedundantPage()
    {
        var viewModel = GetStopRequestViewModel();
        var result = Controller.StopApprenticeship(viewModel);

        var redirect = result.VerifyReturnsRedirectToActionResult();
        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("HasTheApprenticeBeenMadeRedundant"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            Assert.That(viewModel.StopMonth, Is.EqualTo(redirect.RouteValues["StopMonth"]));
            Assert.That(viewModel.StopYear, Is.EqualTo(redirect.RouteValues["StopYear"]));
            Assert.That(viewModel.IsCoPJourney, Is.EqualTo(redirect.RouteValues["IsCoPJourney"]));
        });
    }

    private static StopRequestViewModel GetStopRequestViewModel()
    {
        return new StopRequestViewModel { StopMonth = 6, StopYear = 2020, ApprenticeshipId = 1, AccountHashedId = "AAXX", IsCoPJourney = true, ApprenticeshipHashedId = "BBCVCVS" };
    }
}