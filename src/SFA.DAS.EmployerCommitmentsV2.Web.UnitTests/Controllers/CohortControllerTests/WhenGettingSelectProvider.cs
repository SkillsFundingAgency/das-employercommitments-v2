using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectProvider
{
    [Test, MoqAutoData]
    public async Task ThenMapsTheRequestToViewModel(
        SelectProviderRequest request,
        SelectProviderViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        await controller.SelectProvider(request);

        mockMapper.Verify(x => x.Map<SelectProviderViewModel>(It.IsAny<SelectProviderRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        SelectProviderRequest request,
        SelectProviderViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<SelectProviderViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectProvider(request) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.Null);
            Assert.That(result.Model, Is.SameAs(viewModel));
        });
    }
}