using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGetingAddDraftApprenticeship
{
    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        ApprenticeRequest request,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        controller.TempData = new Mock<ITempDataDictionary>().Object;
        var viewModel = new ApprenticeViewModel();

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.AddDraftApprenticeship(request) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.SameAs("Apprentice"));
            Assert.That(result.Model, Is.SameAs(viewModel));
        });
    }
}