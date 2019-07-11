using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
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
            IndexViewModel viewModel,
            [Frozen] Mock<IMapper<IndexRequest, IndexViewModel>> mockMapper,
            CreateCohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map(indexRequest))
                .Returns(viewModel);

            var result = controller.Index(indexRequest) as ViewResult;

            result.ViewName.Should().BeNull();
            result.Model.Should().Be(viewModel);
        }
    }
}