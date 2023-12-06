using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
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

            _fixture.VerifyView(actionResult);
        }
    }

    public class WhenRequestingApprenticeshipStoppedInformPageTestsFixture : ApprenticeControllerTestFixtureBase
    {
        public WhenRequestingApprenticeshipStoppedInformPageTestsFixture() 
            : base(){}

        public IActionResult ApprenticeshipStoppedInform()
        {
            return _controller.ApprenticeshipStoppedInform();
        }

        public void VerifyView(IActionResult actionResult)
        {
            var viewResult = actionResult as ViewResult;

            Assert.That(viewResult, Is.Not.Null);
        }
    }
}
