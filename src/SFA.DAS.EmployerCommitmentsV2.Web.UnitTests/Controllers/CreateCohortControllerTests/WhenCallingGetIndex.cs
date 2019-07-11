using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    public class WhenCallingGetIndex
    {
        [Test, MoqAutoData]
        public void Then_Returns_View(
            IndexRequest indexRequest,
            CreateCohortController controller)
        {
            var result = controller.Index(indexRequest) as ViewResult;

            result.ViewName.Should().BeNull();
            result.Model.Should().Be(indexRequest);
        }
    }
}