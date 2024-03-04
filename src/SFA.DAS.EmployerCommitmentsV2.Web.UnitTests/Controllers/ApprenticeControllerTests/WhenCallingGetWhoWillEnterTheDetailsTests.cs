using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenCallingGetWhoWillEnterTheDetailsTests
{
    private WhenCallingGetWhoWillEnterTheDetailsTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenCallingGetWhoWillEnterTheDetailsTestsFixture();
    }

    [Test]
    public async Task ThenTheCorrectViewIsReturned()
    {
        var result = await _fixture.WhoWillEnterTheDetails();

        _fixture.VerifyViewModel(result as ViewResult);
    }
}

public class WhenCallingGetWhoWillEnterTheDetailsTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeOfProviderRequest _request;
    private readonly WhoWillEnterTheDetailsViewModel _viewModel;

    public WhenCallingGetWhoWillEnterTheDetailsTestsFixture()
    {
        _request = AutoFixture.Create<ChangeOfProviderRequest>();
        _viewModel = AutoFixture.Create<WhoWillEnterTheDetailsViewModel>();

        MockMapper.Setup(m => m.Map<WhoWillEnterTheDetailsViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> WhoWillEnterTheDetails()
    {
        return await Controller.WhoWillEnterTheDetails(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as WhoWillEnterTheDetailsViewModel;

        Assert.That(viewModel, Is.InstanceOf<WhoWillEnterTheDetailsViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}