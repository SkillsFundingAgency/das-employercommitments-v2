using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetApprovalRequestAlertsTests
{
    private WhenCallingGetApprovalRequestAlertsTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingGetApprovalRequestAlertsTestsFixture();
    }

    [Test]
    public async Task Then_ReturnView()
    {
        var result = await _fixture.GetApprovalRequestAlerts();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingGetApprovalRequestAlertsTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ApprenticeshipApprovalRequestAlertsRequest _request;
    private readonly ApprenticeshipApprovalRequestAlertsViewModel _viewModel;

    public WhenCallingGetApprovalRequestAlertsTestsFixture()
    {
        _request = AutoFixture.Create<ApprenticeshipApprovalRequestAlertsRequest>();
        _viewModel = AutoFixture.Create<ApprenticeshipApprovalRequestAlertsViewModel>();

        MockMapper.Setup(m => m.Map<ApprenticeshipApprovalRequestAlertsViewModel>(It.Is<ApprenticeshipApprovalRequestAlertsRequest>
            (t => t.ApprenticeshipId == _request.ApprenticeshipId
        && t.ApprenticeshipHashedId == _request.ApprenticeshipHashedId
        && t.AccountHashedId == _request.AccountHashedId)))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> GetApprovalRequestAlerts()
    {
        return await Controller.ViewApprovalRequestAlerts(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ApprenticeshipApprovalRequestAlertsViewModel;

        viewModel.Should().BeEquivalentTo(_viewModel);
    }
}