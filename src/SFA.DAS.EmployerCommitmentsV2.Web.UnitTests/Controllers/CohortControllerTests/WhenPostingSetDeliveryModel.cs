using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingSetDeliveryModel
{
    [Test, MoqAutoData]
    public async Task ThenReturnsRedirect(
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        viewModel.DeliveryModels = new Fixture().CreateMany<DeliveryModel>().ToArray();

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
            .ReturnsAsync(request);

        var result = await controller.SetDeliveryModel(viewModel) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("AddDraftApprenticeship"));
    }
}