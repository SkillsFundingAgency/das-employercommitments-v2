using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingApprenticeshipApprovalRequest
{
    private WhenCallingApprenticeshipApprovalRequestFixture _fixture;

    [SetUp]
    public void Arrange() => _fixture = new WhenCallingApprenticeshipApprovalRequestFixture();

    [Test]
    public async Task ThenVerifyMapperWasCalled()
    {
        await _fixture.GetApprovalRequest();

        _fixture.VerifyMapperWasCalled();
    }

    [Test]
    public async Task ThenReturnsViewModel()
    {
        var result = await _fixture.GetApprovalRequest();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task ThenDisplaysMessageIfStatusIsNotLivePauseOrWaitingToStart()
    {
        var result = await _fixture.SetChangeApprovalAllowed(false).GetApprovalRequest();

        _fixture.VerifyChangeApprovalAllowedModelStateError(result as ViewResult);
    }

    [Test]
    public async Task ThenReturnsViewModelIfStatusIsLivePauseOrWaitingToStart()
    {
        var result = await _fixture.SetChangeApprovalAllowed(true).GetApprovalRequest();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task ThenDisplaysMessageIfChangeHasBeenCompleted()
    {
        var result = await _fixture.SetApprovalRequestStatus(CocApprovalResultStatus.Complete).GetApprovalRequest();

        _fixture.VerifyApprovalRequestStatusModelStateError(result as ViewResult);
    }

    [Test]
    public async Task ThenReturnsViewModelIfChangeIsPending()
    {
        var result = await _fixture.SetApprovalRequestStatus(CocApprovalResultStatus.Pending).GetApprovalRequest();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task ThenPostsTheApproval()
    {
        var result = await _fixture.PostApprovalRequest(true);

        result.VerifyReturnsRedirectToActionResult().ActionName.Should().Be("ApprenticeshipApprovalRequestConfirmed");
    }

    [Test]
    public async Task ThenPostsTheDecline()
    {
        var result = await _fixture.PostApprovalRequest(false);

        result.VerifyReturnsRedirectToActionResult().ActionName.Should().Be("GetApprenticeshipApprovalRequest");
    }
}

public class WhenCallingApprenticeshipApprovalRequestFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ApprenticeshipApprovalRequest _request;
    private readonly ApprenticeshipApprovalRequestViewModel _viewModel;
    private Mock<IAuthenticationService> _authenticationService;

    public WhenCallingApprenticeshipApprovalRequestFixture()
    {
        var fixture = new Fixture();

        _request = fixture.Create<ApprenticeshipApprovalRequest>();
        _viewModel = fixture.Create<ApprenticeshipApprovalRequestViewModel>();

        MockMapper.Setup(m => m.Map<ApprenticeshipApprovalRequestViewModel>(_request)).ReturnsAsync(_viewModel);
        ApprovalsApiClientMock.Setup(x => x.ProcessCocApproval(_viewModel.AccountId, _viewModel.ApprenticeshipId, _viewModel.ApprovalRequestId, 
            It.IsAny<ProcessApprenticeshipApprovalRequest>(), It.IsAny<CancellationToken>())).Verifiable();
        _authenticationService = new Mock<IAuthenticationService>();
    }

    public async Task<IActionResult> GetApprovalRequest()
    {
        var result = await Controller.GetApprenticeshipApprovalRequest(_request);

        return result as ViewResult;
    }

    public WhenCallingApprenticeshipApprovalRequestFixture SetApprovalRequestStatus(CocApprovalResultStatus status)
    {
        _viewModel.ApprovalRequestStatus = status;
        return this;
    }

    public WhenCallingApprenticeshipApprovalRequestFixture SetChangeApprovalAllowed(bool changeApprovalAllowed)
    {
        _viewModel.ChangeApprovalAllowed = changeApprovalAllowed;
        return this;
    }

    public void VerifyMapperWasCalled()
    {
        MockMapper.Verify(m => m.Map<ApprenticeshipApprovalRequestViewModel>(_request));
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ApprenticeshipApprovalRequestViewModel;

        viewModel.Should().Be(_viewModel);
    }

    public void VerifyApprovalRequestStatusModelStateError(ViewResult viewResult)
    {
        viewResult.Should().NotBeNull();
        viewResult.ViewData.ModelState.Should().ContainSingle(m => m.Key == "ApprovalRequestStatus" && m.Value.Errors.Any(e => e.ErrorMessage == "This change has already been approved."));
    }

    public async Task<IActionResult> PostApprovalRequest(bool applyApproval)
    {
        _viewModel.ApproveChanges = applyApproval;
        var result = await Controller.PostApprenticeshipApprovalRequest(_authenticationService.Object, _viewModel);

        return result;
    }

    public void VerifyChangeApprovalAllowedModelStateError(ViewResult viewResult)
    {
        viewResult.Should().NotBeNull();
        viewResult.ViewData.ModelState.Should().ContainSingle(m => m.Key == "ChangeApprovalAllowed" && m.Value.Errors.Any(e => e.ErrorMessage == "This change no longer exists"));
    }
}