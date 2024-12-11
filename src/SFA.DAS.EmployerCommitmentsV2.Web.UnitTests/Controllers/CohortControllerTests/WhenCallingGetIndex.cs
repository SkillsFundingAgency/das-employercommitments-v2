using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenCallingGetIndex
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_ViewModel(
        IndexRequest request,
        IndexViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheStorageService
         .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
         .Returns(Task.CompletedTask);

        mockMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(It.IsAny<IndexRequest>()))
            .ReturnsAsync(viewModel);

        var result = await controller.Index(request) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as IndexViewModel;
        model.Should().BeEquivalentTo(viewModel);
    }
}