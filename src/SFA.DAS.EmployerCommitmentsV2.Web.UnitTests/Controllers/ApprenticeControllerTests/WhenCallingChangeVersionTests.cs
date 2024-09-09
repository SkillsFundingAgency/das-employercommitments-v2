using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingChangeVersionTests
{
    private WhenCallingChangeVersionTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingChangeVersionTestsFixture();
    }

    [Test]
    public async Task ThenReturnView()
    {
        var result = await _fixture.ChangeVersion();

        _fixture.VerifyViewModel(result as ViewResult);
    }

    [Test]
    public async Task ThenSelectedVersionSet_WhenEditModelAvailable()
    {
        var version = "1.3";
        _fixture.SetEditApprenticeViewModel(version);
        var result = await _fixture.ChangeVersion();

        WhenCallingChangeVersionTestsFixture.VerifySelectedVersion(result as ViewResult, version);
    }
}

public class WhenCallingChangeVersionTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeVersionRequest _request;
    private readonly ChangeVersionViewModel _viewModel;

    public WhenCallingChangeVersionTestsFixture()
    {
        _request = AutoFixture.Create<ChangeVersionRequest>();
        _viewModel = AutoFixture.Create<ChangeVersionViewModel>();

        MockMapper.Setup(m => m.Map<ChangeVersionViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public void SetEditApprenticeViewModel(string version)
    {
        var editApprenticeViewModel = new EditApprenticeshipRequestViewModel
        {
            Version = version
        };

        _cacheStorageService
            .Setup(x => x.RetrieveFromCache<EditApprenticeshipRequestViewModel>(It.IsAny<string>()))
            .ReturnsAsync(editApprenticeViewModel);
    }

    public async Task<IActionResult> ChangeVersion()
    {
        return await Controller.ChangeVersion(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ChangeVersionViewModel;

        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }

    public static void VerifySelectedVersion(ViewResult viewResult, string version)
    {
        var viewModel = viewResult.Model as ChangeVersionViewModel;

        viewModel.SelectedVersion.Should().Be(version);
    }
}