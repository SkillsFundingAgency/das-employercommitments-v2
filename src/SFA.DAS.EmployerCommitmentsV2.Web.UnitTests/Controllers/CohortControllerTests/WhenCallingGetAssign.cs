using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenCallingGetAssign
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_Model(
        AddApprenticeshipCacheModel cacheModel,
        AssignViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.CacheKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<AssignViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.Assign(cacheModel.CacheKey) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as AssignViewModel;
        model.Should().BeSameAs(viewModel);
    }
}