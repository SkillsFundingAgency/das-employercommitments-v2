using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
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
            .Setup(x => x.RetrieveFromCache<ApprenticeViewModel>(It.IsAny<Guid>()))
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
       GetFundingBandDataResponse fundingDataResponse,
       [Frozen] Mock<IModelMapper> mockMapper,
       [Frozen] Mock<IApprovalsApiClient> mockApprovalsApiClient,
       [Frozen] Mock<ICacheStorageService> cacheStorageService,
       [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();
        var cacheItem = new ApprenticeViewModel
        {
            DeliveryModel = CommitmentsV2.Types.DeliveryModel.Regular,
            CourseCode = "CourseCode",
            CacheKey = request.CacheKey            
        };

        cacheStorageService
            .Setup(x => x.RetrieveFromCache<ApprenticeViewModel>(cacheItem.CacheKey.Value))
            .ReturnsAsync(cacheItem);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(request))
            .ReturnsAsync(viewModel);

        mockApprovalsApiClient
            .Setup(x => x.GetFundingBandDataByCourseCodeAndStartDate(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundingDataResponse);

        var result = await controller.AddDraftApprenticeship(request) as ViewResult;
        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");

        var resultObject = result.Model as ApprenticeViewModel;

        resultObject.Should().NotBeNull();
        resultObject.DeliveryModel.Should().Be(cacheItem.DeliveryModel);
        resultObject.CourseCode.Should().Be(cacheItem.CourseCode);
        resultObject.StandardPageUrl.Should().Be(fundingDataResponse.StandardPageUrl);
        resultObject.FundingBandMax.Should().Be(fundingDataResponse.ProposedMaxFunding);
    }
}