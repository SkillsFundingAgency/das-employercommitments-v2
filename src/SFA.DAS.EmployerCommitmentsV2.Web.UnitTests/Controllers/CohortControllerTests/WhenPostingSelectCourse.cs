﻿using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingSelectCourse
{
    [Test, MoqAutoData]
    public async Task ThenReturnsRedirect(
        ApprenticeRequest request,
        SelectCourseViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
            .ReturnsAsync(request);

        var result = await controller.SelectCourse(viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("SelectDeliveryModel"));
    }
}