using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingSelectDeliveryModel
{
    [Test, MoqAutoData]
    public async Task WithDeliveryModelsThenReturnsView(
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        viewModel.DeliveryModels = new Fixture().CreateMany<DeliveryModel>().ToArray();

        mockMapper
            .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDeliveryModel(request) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.SameAs("SelectDeliveryModel"));
            Assert.That(result.Model, Is.SameAs(viewModel));
        });
    }

    [Test, MoqAutoData]
    public async Task WithOutDeliveryModelsThenReturnsRedirect(
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        viewModel.DeliveryModels = Array.Empty<DeliveryModel>();

        mockMapper
            .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDeliveryModel(request) as RedirectToActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ActionName, Is.EqualTo("AddDraftApprenticeship"));
    }
}