using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectAcceptedLevyConnections
{
    [Test, MoqAutoData]
    public async Task WithSelectAcceptedLevyTransfersThenReturnsView(
        AddApprenticeshipCacheModel cacheModel,
        SelectAcceptedLevyTransferConnectionViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<SelectAcceptedLevyTransferConnectionViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectAcceptedLevyTransferConnection(cacheModel.ApprenticeshipSessionKey) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(viewModel);
    }
}