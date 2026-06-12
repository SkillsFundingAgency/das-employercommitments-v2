using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingChangeHistoryTests
{
    WhenCallingChangeHistoryTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingChangeHistoryTestsFixture();
    }

    [Test]
    public async Task Then_ReturnView()
    {
        var result = await _fixture.GetChangeHistory();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingChangeHistoryTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeHistoryRequest _request;
    private readonly ChangeHistoryListViewModel _viewModel;

    public WhenCallingChangeHistoryTestsFixture()
    {
        _request = AutoFixture.Create<ChangeHistoryRequest>();
        _viewModel = AutoFixture.Create<ChangeHistoryListViewModel>();

        MockMapper.Setup(m => m.Map<ChangeHistoryListViewModel>(It.IsAny<ChangeHistoryRequest>()))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> GetChangeHistory()
    {
        return await Controller.ChangeHistory(_request.ApprenticeshipHashedId, _request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ChangeHistoryListViewModel;

        viewModel.Should().BeEquivalentTo(_viewModel);
    }
}
