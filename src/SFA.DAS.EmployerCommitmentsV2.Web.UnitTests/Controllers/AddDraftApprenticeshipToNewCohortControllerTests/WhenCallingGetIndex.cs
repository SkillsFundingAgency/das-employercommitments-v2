using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.AddDraftApprenticeshipToNewCohortControllerTests
{
    public class WhenCallingGetIndex
    {
        [Test, MoqAutoData]
        public void Then_Returns_View(
            RouteModel routeModel,
            AddDraftApprenticeshipToNewCohortController controller)
        {
            var result = controller.Index(routeModel) as ViewResult;

            result.ViewName.Should().BeNull();
            result.Model.Should().Be(routeModel);
        }
    }
}