using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingTheViewChangesPageTests
{
    private WhenCallingTheViewChangesPageTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingTheViewChangesPageTestsFixture();
    }

    [Test]
    public async Task ThenViewIsReturned()
    {
        var actionResult = await _fixture.ViewChanges();

        _fixture.VerifyViewModel(actionResult);
    }
}

public class WhenCallingTheViewChangesPageTestsFixture
{
    private readonly ViewChangesRequest _request;
    private readonly ViewChangesViewModel _viewModel;
    private readonly ApprenticeController _controller;

    public WhenCallingTheViewChangesPageTestsFixture()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<ViewChangesRequest>();
        _viewModel = autoFixture.Create<ViewChangesViewModel>();

        var mockMapper = new Mock<IModelMapper>();
        mockMapper.Setup(m => m.Map<ViewChangesViewModel>(_request))
            .ReturnsAsync(_viewModel);

        _controller = new ApprenticeController(mockMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<ApprenticeController>>());
    }

    public async Task<IActionResult> ViewChanges()
    {
        return await _controller.ViewChanges(_request);
    }

    public void VerifyViewModel(IActionResult actionResult)
    {
        var result = actionResult as ViewResult;
        var viewModel = result.Model;

        Assert.That(viewModel, Is.InstanceOf<ViewChangesViewModel>());

        var viewChangesViewModelResult = viewModel as ViewChangesViewModel;

        Assert.That(viewChangesViewModelResult, Is.EqualTo(_viewModel));
    }
}