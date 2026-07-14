using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetAllChangeHistory
{
    private WhenGettingAllChangeHistoryFixture _fixture;

    [SetUp]
    public void Arrange() => _fixture = new WhenGettingAllChangeHistoryFixture();

    [Test]
    public async Task ThenVerifyMapperWasCalled()
    {
        await _fixture.GetAllChangeHistory();

        _fixture.VerifyMapperWasCalled();
    }

    [Test]
    public async Task ThenReturnsViewModel()
    {
        var result = await _fixture.GetAllChangeHistory();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenGettingAllChangeHistoryFixture : ApprenticeControllerTestFixtureBase
{
    private readonly GetAllChangeHistoryRequest _request;
    private readonly GetAllChangeHistoryListViewModel _viewModel;

    public WhenGettingAllChangeHistoryFixture()
    {
        var fixture = new Fixture();

        _request = fixture.Create<GetAllChangeHistoryRequest>();
        _viewModel = fixture.Create<GetAllChangeHistoryListViewModel>();

        MockMapper.Setup(m => m.Map<GetAllChangeHistoryListViewModel>(_request)).ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> GetAllChangeHistory()
    {
        var result = await Controller.GetAllChangeHistory(_request);

        return result as ViewResult;
    }

    public void VerifyMapperWasCalled()
    {
        MockMapper.Verify(m => m.Map<GetAllChangeHistoryListViewModel>(_request));
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as GetAllChangeHistoryListViewModel;

        viewModel.Should().Be(_viewModel);
    }
}