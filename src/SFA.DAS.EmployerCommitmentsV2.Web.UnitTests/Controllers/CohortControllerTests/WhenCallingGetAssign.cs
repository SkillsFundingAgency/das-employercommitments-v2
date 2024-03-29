﻿using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenCallingGetAssign
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_Model(
        AssignRequest request,
        AssignViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<AssignViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.Assign(request) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as AssignViewModel;
        model.Should().BeSameAs(viewModel);
    }
}