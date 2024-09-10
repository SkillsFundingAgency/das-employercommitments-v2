using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingConfirmEditApprenticeshipTests
{
    private WhenRequestingConfirmEditApprenticeshipFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingConfirmEditApprenticeshipFixture();
    }

    [Test]
    public async Task VerifyViewModelMapperIsCalled()
    {
        await _fixture.ConfirmEditApprenticeship();
        _fixture.VerifyViewModelMapperIsCalled();
    }

    [Test]
    public async Task VerifyViewIsReturned()
    {
        var result = await _fixture.ConfirmEditApprenticeship();
        WhenRequestingConfirmEditApprenticeshipFixture.VerifyViewResultIsReturned(result);
    }
}

public class WhenRequestingConfirmEditApprenticeshipFixture : ApprenticeControllerTestFixtureBase
{
    private readonly EditApprenticeshipRequestViewModel _viewModel;
    private readonly ConfirmEditApprenticeshipRequest _request;

    public WhenRequestingConfirmEditApprenticeshipFixture() : base()
    {
        var fixture = new Fixture();
        _request = fixture.Create<ConfirmEditApprenticeshipRequest>();

        _viewModel = fixture.Build<EditApprenticeshipRequestViewModel>().Without(x => x.BirthDay).Without(x => x.BirthMonth).Without(x => x.BirthYear)
            .Without(x => x.StartMonth).Without(x => x.StartYear).Without(x => x.StartDate)
            .Without(x => x.EndDate).Without(x => x.EndMonth).Without(x => x.EndYear)
            .Without(x => x.EmploymentEndDate).Without(x => x.EmploymentEndMonth).Without(x => x.EmploymentEndYear)
            .Create();

        _cacheStorageService.Setup(x => x.RetrieveFromCache<EditApprenticeshipRequestViewModel>(It.IsAny<Guid>()))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> ConfirmEditApprenticeship()
    {
        return await Controller.ConfirmEditApprenticeship(_request);
    }

    internal void VerifyViewModelMapperIsCalled()
    {
        MockMapper.Verify(x => x.Map<ConfirmEditApprenticeshipViewModel>(It.IsAny<EditApprenticeshipRequestViewModel>()), Times.Once());
    }

    internal static void VerifyViewResultIsReturned(IActionResult result)
    {
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}