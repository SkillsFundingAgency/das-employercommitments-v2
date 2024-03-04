using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

[TestFixture]
public class WhenPostingHasTheApprenticeBeenMadeRedundant : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        Controller = new ApprenticeController(Mock.Of<IModelMapper>(),
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test, MoqAutoData]
    public void AndTheUserAnswered_HasTheApprenticeBeenMadeRedundant_ThenRedirectToConfirmStopApprenticeship(MadeRedundantViewModel viewModel)
    {
        var result = Controller.HasTheApprenticeBeenMadeRedundant(viewModel);

        var redirect = result.VerifyReturnsRedirectToActionResult();
        Assert.Multiple(() =>
        {
            Assert.That(redirect.ActionName, Is.EqualTo("ConfirmStop"));
            Assert.That(viewModel.AccountHashedId, Is.EqualTo(redirect.RouteValues["AccountHashedId"]));
            Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(redirect.RouteValues["ApprenticeshipHashedId"]));
            Assert.That(viewModel.StopMonth, Is.EqualTo(redirect.RouteValues["StopMonth"]));
            Assert.That(viewModel.StopYear, Is.EqualTo(redirect.RouteValues["StopYear"]));
            Assert.That(viewModel.IsCoPJourney, Is.EqualTo(redirect.RouteValues["IsCoPJourney"]));
            Assert.That(viewModel.MadeRedundant, Is.EqualTo(redirect.RouteValues["MadeRedundant"]));
        });
    }
}