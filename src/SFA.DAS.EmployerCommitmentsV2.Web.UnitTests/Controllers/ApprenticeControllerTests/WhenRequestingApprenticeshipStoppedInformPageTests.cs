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
    public IActionResult ApprenticeshipStoppedInform()
    {
        return Controller.ApprenticeshipStoppedInform();
    }

    public static void VerifyView(IActionResult actionResult)
    {
        var viewResult = actionResult as ViewResult;

        Assert.That(viewResult, Is.Not.Null);
    }
}