using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenGettingSelectDeliveryModel
{
    [Test, MoqAutoData]
    public async Task WithDeliveryModelsThenReturnsView(
        AddApprenticeshipCacheModel cacheModel,
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.AddApprenticeshipCacheKey = request.AddApprenticeshipCacheKey.Value;

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.AddApprenticeshipCacheKey))
           .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        viewModel.DeliveryModels = new Fixture().CreateMany<DeliveryModel>().ToArray();

        mockMapper
            .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDeliveryModel(request) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().Be("SelectDeliveryModel");
        result.Model.Should().BeEquivalentTo(viewModel);
    }

    [Test, MoqAutoData]
    public async Task WithOutDeliveryModelsThenReturnsRedirect(
        AddApprenticeshipCacheModel cacheModel,
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        viewModel.DeliveryModels = [];
        cacheModel.AddApprenticeshipCacheKey = request.AddApprenticeshipCacheKey.Value;

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.AddApprenticeshipCacheKey))
          .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        mockMapper
            .Setup(mapper => mapper.Map<SelectDeliveryModelViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectDeliveryModel(request) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("AddDraftApprenticeship");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["AddApprenticeshipCacheKey"].Should().Be(cacheModel.AddApprenticeshipCacheKey);
    }
}