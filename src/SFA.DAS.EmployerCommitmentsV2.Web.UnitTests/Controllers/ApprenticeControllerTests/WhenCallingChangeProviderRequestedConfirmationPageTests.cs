using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingChangeProviderRequestedConfirmationPageTests
{
    private WhenCallingChangeProviderRequestedConfirmationPageTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingChangeProviderRequestedConfirmationPageTestsFixture();

    }

    [Test]
    public async Task ThenViewIsReturned()
    {
        var result = await _fixture.ChangeProviderRequested();

        _fixture.VerifyViewModel(result);
    }
}

public class WhenCallingChangeProviderRequestedConfirmationPageTestsFixture
{
    private readonly ChangeProviderRequestedConfirmationRequest _request;
    private readonly ChangeProviderRequestedConfirmationViewModel _viewModel;
    protected readonly Mock<ICacheStorageService> _cacheStorageService;

    private readonly Mock<IModelMapper> _mockMapper;
    private readonly ApprenticeController _controller;

    public WhenCallingChangeProviderRequestedConfirmationPageTestsFixture()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<ChangeProviderRequestedConfirmationRequest>();
        _viewModel = autoFixture.Create<ChangeProviderRequestedConfirmationViewModel>();
        _cacheStorageService = new Mock<ICacheStorageService>();

        _mockMapper = new Mock<IModelMapper>();
        _mockMapper.Setup(m => m.Map<ChangeProviderRequestedConfirmationViewModel>(_request))
            .ReturnsAsync(_viewModel);

        _controller = new ApprenticeController(_mockMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            _cacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>());                
    }

    public async Task<IActionResult> ChangeProviderRequested()
    {
        return await _controller.ChangeProviderRequested(_request);
    } 

    public void VerifyViewModel(IActionResult actionResult)
    {
        var result = actionResult as ViewResult;
        var viewModel = result.Model;

        Assert.That(viewModel, Is.InstanceOf<ChangeProviderRequestedConfirmationViewModel>());

        var changeProviderRequestedViewModel = viewModel as ChangeProviderRequestedConfirmationViewModel;

        Assert.That(changeProviderRequestedViewModel, Is.EqualTo(_viewModel));
    }
}