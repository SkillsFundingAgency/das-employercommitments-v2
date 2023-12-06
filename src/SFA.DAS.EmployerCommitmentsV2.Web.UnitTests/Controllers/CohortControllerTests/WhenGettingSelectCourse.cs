using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenGettingSelectCourse
    {
        [Test, MoqAutoData]
        public async Task ThenReturnsView(
            ApprenticeRequest request,
            SelectCourseViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Greedy] CohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<SelectCourseViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.SelectCourse(request) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.SameAs("SelectCourse"));
            Assert.That(result.Model, Is.SameAs(viewModel));
        }
    }
}
