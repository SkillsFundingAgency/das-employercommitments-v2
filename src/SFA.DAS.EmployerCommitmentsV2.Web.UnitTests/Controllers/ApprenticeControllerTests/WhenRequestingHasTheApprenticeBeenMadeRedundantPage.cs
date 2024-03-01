using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingHasTheApprenticeBeenMadeRedundantPage : ApprenticeControllerTestBase
{
    [SetUp]
    public void Arrange()
    {
        MockModelMapper = new Mock<IModelMapper>();
        MockCookieStorageService = new Mock<Interfaces.ICookieStorageService<IndexRequest>>();
        MockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        Controller = new ApprenticeController(MockModelMapper.Object,
            MockCookieStorageService.Object,
            MockCommitmentsApiClient.Object,
            Mock.Of<ILogger<ApprenticeController>>());
    }

    [Test, MoqAutoData]
    public async Task WhenRequesting_HasTheAppretniceBeenMadeRedundant_ThenMadeRedundantViewModelIsPassedToTheView(MadeRedundantViewModel expectedViewModel)
    {
        MockModelMapper.Setup(m => m.Map<MadeRedundantViewModel>(It.IsAny<MadeRedundantRequest>()))
            .ReturnsAsync(expectedViewModel);

        var viewResult = await Controller.HasTheApprenticeBeenMadeRedundant(new MadeRedundantRequest()) as ViewResult;
        var viewModel = viewResult.Model;

        var actualViewModel = (MadeRedundantViewModel)viewModel;

        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<MadeRedundantViewModel>());
            Assert.That(actualViewModel, Is.EqualTo(expectedViewModel));
        });
    }
}