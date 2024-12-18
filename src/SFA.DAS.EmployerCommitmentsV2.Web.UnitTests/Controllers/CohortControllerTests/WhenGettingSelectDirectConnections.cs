using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectDirectConnections
{
    [Test, MoqAutoData]
    public async Task WithSelectDirectTransfersThenReturnsView(
        BaseSelectProviderRequest request,
        SelectTransferConnectionViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<SelectTransferConnectionViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDirectTransferConnection(request) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(viewModel);
    }
}