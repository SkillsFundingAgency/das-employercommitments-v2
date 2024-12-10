using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectProvider
{
    [Test, MoqAutoData]
    public async Task ThenMapsTheRequestToViewModel(
        AddApprenticeshipCacheModel cacheModel,
        SelectProviderViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<IEncodingService> mockEncodingService,
        [Greedy] CohortController controller)
    {
        long accountLegalEntityId = 123;
        mockEncodingService
            .Setup(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId))
            .Returns(true);

        mockMapper
            .Setup(x => x.Map<SelectProviderViewModel>(It.IsAny<AddApprenticeshipCacheModel>()))
            .ReturnsAsync(viewModel);

        await controller.SelectProvider(cacheModel.CacheKey);
        mockMapper.Verify(x => x.Map<SelectProviderViewModel>(It.IsAny<SelectProviderRequest>()), Times.Once);
        mockEncodingService.Verify(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        SelectProviderRequest request,
        SelectProviderViewModel viewModel,
        [Frozen] Mock<IEncodingService> mockEncodingService,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        long accountLegalEntityId = 123;
        mockEncodingService
            .Setup(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId))
            .Returns(true);

        mockMapper
            .Setup(mapper => mapper.Map<SelectProviderViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectProvider(cacheModel.CacheKey) as ViewResult;

        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.Null);
            Assert.That(result.Model, Is.SameAs(viewModel));
        });
    }
}