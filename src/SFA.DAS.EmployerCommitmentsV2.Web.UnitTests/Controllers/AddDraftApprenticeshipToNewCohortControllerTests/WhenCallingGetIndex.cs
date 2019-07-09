using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.AddDraftApprenticeshipToNewCohortControllerTests
{
    public class WhenCallingGetIndex
    {
        [Test, MoqAutoData]
        public void Then_Returns_View(
            string accountId,
            AddDraftApprenticeshipToNewCohortController controller)
        {
            var result = controller.Index(accountId) as ViewResult;

            result.ViewName.Should().BeNull();
        }
    }
}