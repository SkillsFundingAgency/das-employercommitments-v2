using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectAcceptedLevyConnections
{
    [Test, MoqAutoData]
    public async Task WithSelectAcceptedLevyTransfersThenReturnsView(
        BaseSelectProviderRequest request,
        SelectAcceptedLevyTransferConnectionViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<SelectAcceptedLevyTransferConnectionViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectAcceptedLevyTransferConnection(request) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(viewModel);
    }
}