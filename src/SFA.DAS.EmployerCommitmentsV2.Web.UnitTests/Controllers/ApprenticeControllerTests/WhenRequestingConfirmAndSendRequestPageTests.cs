using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingConfirmAndSendRequestPageTests
{
    private WhenRequestingConfirmAndSendRequestPageTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingConfirmAndSendRequestPageTestsFixture();
    }

    [Test]
    public async Task ThenCorrectViewIsReturned()
    {
        var actionResult = await _fixture.ConfirmDetailsAndSendRequest();

        _fixture.VerifyViewModel(actionResult as ViewResult) ;
    }
}

public class WhenRequestingConfirmAndSendRequestPageTestsFixture : ApprenticeControllerTestFixtureBase
{
    private readonly ChangeOfProviderRequest _request;
    private readonly ConfirmDetailsAndSendViewModel _viewModel;

    public WhenRequestingConfirmAndSendRequestPageTestsFixture() : base()
    {
        _request = AutoFixture.Create<ChangeOfProviderRequest>();
        _viewModel = AutoFixture.Create<ConfirmDetailsAndSendViewModel>();

            
        MockMapper.Setup(m => m.Map<ConfirmDetailsAndSendViewModel>(_request))
            .ReturnsAsync(_viewModel);
    }

    public async Task<IActionResult> ConfirmDetailsAndSendRequest()
    {
        return await Controller.ConfirmDetailsAndSendRequestPage(_request);
    }

    public void VerifyViewModel(ViewResult viewResult)
    {
        var viewModel = viewResult.Model as ConfirmDetailsAndSendViewModel;

        Assert.That(viewModel, Is.InstanceOf<ConfirmDetailsAndSendViewModel>());
        Assert.That(viewModel, Is.EqualTo(_viewModel));
    }
}