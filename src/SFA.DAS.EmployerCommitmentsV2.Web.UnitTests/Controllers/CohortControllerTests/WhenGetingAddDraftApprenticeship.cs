using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGetingAddDraftApprenticeship
{
    [Test, MoqAutoData]
    public async Task ThenReturnsViewWhenModelIsInCache(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();

        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.AddApprenticeshipCacheKey))
            .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.AddDraftApprenticeship(cacheModel.AddApprenticeshipCacheKey) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");

        var resultObject = result.Model as ApprenticeViewModel;

        resultObject.Should().NotBeNull();
        resultObject.Should().BeEquivalentTo(viewModel);
    }
}