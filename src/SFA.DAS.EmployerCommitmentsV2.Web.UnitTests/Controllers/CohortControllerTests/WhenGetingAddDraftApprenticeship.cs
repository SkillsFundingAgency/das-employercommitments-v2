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
    public async Task ThenReturnsViewWhenNoModelInCache(
        ApprenticeRequest request,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();

        cacheStorageService
            .Setup(x => x.SafeRetrieveFromCache<ApprenticeViewModel>(nameof(ApprenticeViewModel)))
            .ReturnsAsync((ApprenticeViewModel)null);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.AddDraftApprenticeship(request) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");
        result.Model.Should().BeEquivalentTo(viewModel);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsViewWhenModelIsInCache(
       ApprenticeRequest request,
       [Frozen] Mock<IModelMapper> mockMapper,
       [Frozen] Mock<ICacheStorageService> cacheStorageService,
       [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();
        var cacheItem = new ApprenticeViewModel
        {
            DeliveryModel = CommitmentsV2.Types.DeliveryModel.Regular,
            CourseCode = "CourseCode"
        };

        cacheStorageService
            .Setup(x => x.SafeRetrieveFromCache<ApprenticeViewModel>(nameof(ApprenticeViewModel)))
            .ReturnsAsync(cacheItem);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.AddDraftApprenticeship(request) as ViewResult;
        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");

        var resultObject = result.Model as ApprenticeViewModel;

        resultObject.Should().NotBeNull();
        resultObject.DeliveryModel.Should().Be(cacheItem.DeliveryModel);
        resultObject.CourseCode.Should().Be(cacheItem.CourseCode);

    }
}