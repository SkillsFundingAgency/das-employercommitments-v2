using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
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
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        long accountLegalEntityId = 123;
        mockEncodingService
            .Setup(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId))
            .Returns(true);

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(x => x.Map<SelectProviderViewModel>(It.IsAny<AddApprenticeshipCacheModel>()))
            .ReturnsAsync(viewModel);

        await controller.SelectProvider(cacheModel.ApprenticeshipSessionKey);
        mockMapper.Verify(x => x.Map<SelectProviderViewModel>(It.IsAny<AddApprenticeshipCacheModel>()), Times.Once);
        mockEncodingService.Verify(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        SelectProviderViewModel viewModel,
        [Frozen] Mock<IEncodingService> mockEncodingService,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        long accountLegalEntityId = 123;
        mockEncodingService
            .Setup(x => x.TryDecode(cacheModel.AccountLegalEntityHashedId, EncodingType.PublicAccountLegalEntityId, out accountLegalEntityId))
            .Returns(true);

        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<SelectProviderViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectProvider(cacheModel.ApprenticeshipSessionKey) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().BeNull();
        result.Model.Should().BeEquivalentTo(viewModel);       
    }
}