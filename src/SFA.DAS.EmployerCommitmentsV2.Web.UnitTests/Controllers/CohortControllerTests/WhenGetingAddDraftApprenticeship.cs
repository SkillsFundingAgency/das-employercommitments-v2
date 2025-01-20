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
    public async Task ThenReturnsViewWhenModelIsInCacheAndNoCoursePresent(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        var viewModel = new ApprenticeViewModel();

        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(It.IsAny<Guid>()))
            .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.AddDraftApprenticeship(Guid.NewGuid()) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");
        result.Model.Should().BeEquivalentTo(viewModel);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsViewWhenModelIsInCacheAndHasCoursePresent(
       ApprenticeRequest request,
       AddApprenticeshipCacheModel cacheModel,
       GetFundingBandDataResponse fundingDataResponse,
       [Frozen] Mock<IModelMapper> mockMapper,
       [Frozen] Mock<IApprovalsApiClient> mockApprovalsApiClient,
       [Frozen] Mock<ICacheStorageService> cacheStorageService,
       [Greedy] CohortController controller)
    {
        var viewModel= new ApprenticeViewModel
        {
            DeliveryModel = CommitmentsV2.Types.DeliveryModel.Regular,
            CourseCode = "CourseCode"
        };
        cacheStorageService
            .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
            .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        mockMapper
            .Setup(mapper => mapper.Map<ApprenticeViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        mockApprovalsApiClient
            .Setup(x => x.GetFundingBandDataByCourseCodeAndStartDate(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundingDataResponse);

        var result = await controller.AddDraftApprenticeship(cacheModel.ApprenticeshipSessionKey) as ViewResult;
        result.Should().NotBeNull();
        result.ViewName.Should().Be("Apprentice");

        var resultObject = result.Model as ApprenticeViewModel;

        resultObject.Should().NotBeNull();
        resultObject.Should().BeEquivalentTo(viewModel);
        resultObject.DeliveryModel.Should().Be(viewModel.DeliveryModel);
        resultObject.CourseCode.Should().Be(viewModel.CourseCode);
        resultObject.StandardPageUrl.Should().Be(fundingDataResponse.StandardPageUrl);
        resultObject.FundingBandMax.Should().Be(fundingDataResponse.ProposedMaxFunding);
    }
}