using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingEditApprenticeshipTests
{
    private WhenCallingEditApprenticeshipTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingEditApprenticeshipTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.EditApprenticeship();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task AndWeHaveAnExistingEditViewModel_ThenTheCachedModelIsPassedToTheView()
    {
        var result = await _fixture.WithCachedModel().EditApprenticeship();
        _fixture.VerifyViewModelIsEquivalentToCachedViewModel(result as ViewResult);
    }
}

public class WhenCallingEditApprenticeshipTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly EditApprenticeshipRequest _request;
    private readonly EditApprenticeshipRequestViewModel _viewModel;


    public WhenCallingEditApprenticeshipTestsFixture() : base()
    {
        _request = AutoFixture.Create<EditApprenticeshipRequest>();
        _viewModel = AutoFixture.Build<EditApprenticeshipRequestViewModel>()
            .Without(x => x.StartDate).Without(x => x.StartMonth).Without(x => x.StartYear)
            .Without(x => x.EndDate).Without(x => x.EndMonth).Without(x => x.EndYear)
            .Without(x => x.EmploymentEndDate).Without(x => x.EmploymentEndMonth).Without(x => x.EmploymentEndYear)
            .Create();

        MockMapper.Setup(m => m.Map<EditApprenticeshipRequestViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> EditApprenticeship()
    {
        return await Controller.EditApprenticeship(_request);
    }

    public WhenCallingEditApprenticeshipTestsFixture WithCachedModel()
    {
        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<EditApprenticeshipRequestViewModel>(It.IsAny<string>()))
            .ReturnsAsync(_viewModel);
        return this;
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as EditApprenticeshipRequestViewModel;

        Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipRequestViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }

    public void VerifyViewModelIsEquivalentToCachedViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as EditApprenticeshipRequestViewModel;

        Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipRequestViewModel>());
        _viewModel.Should().BeEquivalentTo(viewModel);
    }
}