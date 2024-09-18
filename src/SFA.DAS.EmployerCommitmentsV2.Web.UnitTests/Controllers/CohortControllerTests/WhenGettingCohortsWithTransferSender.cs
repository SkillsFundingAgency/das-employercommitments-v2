using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingCohortsWithTransferSender
{
    private WhenGettingCohortsWithTransferSenderFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenGettingCohortsWithTransferSenderFixture();
    }

    [Test]
    public async Task ThenViewModelShouldBeMappedFromRequest()
    {
        await _fixture.GetCohortsWithTransferSender();
        _fixture.VerifyViewModelIsMappedFromRequest();
    }
}

public class WhenGettingCohortsWithTransferSenderFixture
{
    private readonly CohortsByAccountRequest _request;
    private readonly WithTransferSenderViewModel _viewModel;
    private IActionResult _result;

    public WhenGettingCohortsWithTransferSenderFixture()
    {
        var autoFixture = new Fixture();

        _request = autoFixture.Create<CohortsByAccountRequest>();
        _viewModel = autoFixture.Create<WithTransferSenderViewModel>();

        var modelMapper = new Mock<IModelMapper>();
        modelMapper.Setup(x => x.Map<WithTransferSenderViewModel>(It.Is<CohortsByAccountRequest>(r => r == _request)))
            .ReturnsAsync(_viewModel);

        CohortController = new CohortController(Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            modelMapper.Object,
            Mock.Of<IEncodingService>(),
            Mock.Of<IApprovalsApiClient>(),
            Mock.Of<ICacheStorageService>());
    }

    public CohortController CohortController { get; set; }

    public async Task GetCohortsWithTransferSender()
    {
        _result = await CohortController.WithTransferSender(_request);
    }

    public void VerifyViewModelIsMappedFromRequest()
    {
        var viewResult = (ViewResult)_result;
        var viewModel = viewResult.Model;

        Assert.That(viewModel, Is.InstanceOf<WithTransferSenderViewModel>());
        var WithTransferSenderViewModel = (WithTransferSenderViewModel)viewModel;

        Assert.That(WithTransferSenderViewModel, Is.EqualTo(_viewModel));
    }
}