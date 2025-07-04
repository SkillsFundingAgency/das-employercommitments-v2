using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetSendRequestNewTrainingProviderTests
{
    WhenCallingGetSendRequestNewTrainingProviderTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingGetSendRequestNewTrainingProviderTestsFixture();
    }

    [Test]
    public async Task Then_The_View_Is_Returned()
    {
        var actionResult = await _fixture.SendRequestNewTrainingProvider();

        _fixture.VerifyViewModel(actionResult);
    }
}

public class WhenCallingGetSendRequestNewTrainingProviderTestsFixture
{
    private readonly Mock<IModelMapper> _modelMapper;
    protected readonly Mock<ICacheStorageService> _cacheStorageService;
    protected readonly Mock<IApprovalsApiClient> _approvalsApiClient;
    private ApprenticeController _controller;
    private SendNewTrainingProviderViewModel _expectedViewModel;
    private SendNewTrainingProviderRequest _request;

    public WhenCallingGetSendRequestNewTrainingProviderTestsFixture()
    {
        var autoFixture = new Fixture();
        _expectedViewModel = autoFixture.Create<SendNewTrainingProviderViewModel>();
        _request = autoFixture.Create<SendNewTrainingProviderRequest>();

        _cacheStorageService = new Mock<ICacheStorageService>();
        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        
        _modelMapper = new Mock<IModelMapper>();
        _modelMapper
            .Setup(mapper => mapper.Map<SendNewTrainingProviderViewModel>(_request))
            .ReturnsAsync(_expectedViewModel);

        _controller = new ApprenticeController(_modelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            _cacheStorageService.Object,
            Mock.Of<ILogger<ApprenticeController>>(),
            _approvalsApiClient.Object);
    }

    public async Task<IActionResult> SendRequestNewTrainingProvider()
    {
        return await _controller.SendRequestNewTrainingProvider(_request);
    }

    public void VerifyViewModel(IActionResult actionResult)
    {
        var result = actionResult as ViewResult;
        var viewModel = result.Model;

        Assert.That(viewModel, Is.InstanceOf<SendNewTrainingProviderViewModel>());
        var sendNewTrainingProviderViewModel = (SendNewTrainingProviderViewModel)viewModel;

        Assert.That(sendNewTrainingProviderViewModel, Is.EqualTo(_expectedViewModel));
    }
}