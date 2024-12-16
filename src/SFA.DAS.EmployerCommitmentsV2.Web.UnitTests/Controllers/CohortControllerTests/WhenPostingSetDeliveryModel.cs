using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingSetDeliveryModel
{
    [Test, MoqAutoData]
    public async Task ThenReturnsRedirect(
        AddApprenticeshipCacheModel cacheModel,
        ApprenticeRequest request,
        SelectDeliveryModelViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        viewModel.DeliveryModels = new Fixture().CreateMany<DeliveryModel>().ToArray();
        cacheModel.ApprenticeshipSessionKey = viewModel.ApprenticeshipSessionKey.Value;

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);
        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeRequest>(viewModel))
            .ReturnsAsync(request);

        var result = await controller.SetDeliveryModel(viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("AddDraftApprenticeship");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }
}