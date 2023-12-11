using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingConfirmApprenticeshipHasValidEndDatePage
{
    private WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture();
    }

    [Test]
    public async Task WhenRequesting_ConfirmApprenticeshipHasValidEndDate_ThenConfirmHasValidEndDateRequestViewModelIsPassedToTheView()
    {
        var actionResult = await _fixture.ConfirmHasValidEndDate();

        _fixture.VerifyViewModel(actionResult as ViewResult);
    }
}
public class WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ConfirmHasValidEndDateRequest _request;
    private readonly ConfirmHasValidEndDateViewModel _viewModel;
    public WhenRequestingConfirmApprenticeshipHasValidEndDatePageFixture() : base()
    {
        _request = AutoFixture.Create<ConfirmHasValidEndDateRequest>();
        _viewModel = AutoFixture.Create<ConfirmHasValidEndDateViewModel>();


        MockMapper.Setup(m => m.Map<ConfirmHasValidEndDateViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> ConfirmHasValidEndDate()
    {
        return await Controller.ConfirmHasValidEndDate(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ConfirmHasValidEndDateViewModel;

        Assert.That(viewModel, Is.InstanceOf<ConfirmHasValidEndDateViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}