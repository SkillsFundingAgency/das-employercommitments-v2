using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenRequestingApprenticeshipStoppedInformPageTests
{
    private WhenRequestingApprenticeshipStoppedInformPageTestsFixture _fixture;

    [SetUp]
    public void Arrange()
    {
        _fixture = new WhenRequestingApprenticeshipStoppedInformPageTestsFixture();
    }

    [Test]
    public void ThenCorrectViewIsReturned()
    {
        var actionResult = _fixture.ApprenticeshipStoppedInform();

        WhenRequestingApprenticeshipStoppedInformPageTestsFixture.VerifyView(actionResult);
    }
}

public class WhenRequestingApprenticeshipStoppedInformPageTestsFixture : ApprenticeControllerTestFixtureBase
{
    private const string AccountHashedId = "SDSDKJE£WF";
    private const string ApprenticeshipHashedId = "df4kjss89HG£WF";
    
    public IActionResult ApprenticeshipStoppedInform()
    {
        return Controller.ApprenticeshipStoppedInform(new ApprenticeshipStopInformRequest
        {
            AccountHashedId = AccountHashedId,
            ApprenticeshipHashedId = ApprenticeshipHashedId
        });
    }

    public static void VerifyView(IActionResult actionResult)
    {
        var viewResult = actionResult as ViewResult;

        viewResult.Should().NotBeNull();
        
        var viewModel = viewResult.Model as ApprenticeshipStopInformViewModel;
        viewModel.AccountHashedId.Should().Be(AccountHashedId);
        viewModel.ApprenticeshipHashedId.Should().Be(ApprenticeshipHashedId);
    }
}