using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectDirectConnections
{
    [Test, MoqAutoData]
    public async Task WithSelectDirectTransfersThenReturnsView(
        AddApprenticeshipCacheModel cacheModel,
        SelectTransferConnectionViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<SelectTransferConnectionViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDirectTransferConnection(cacheModel.ApprenticeshipSessionKey) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(viewModel);
    }
}