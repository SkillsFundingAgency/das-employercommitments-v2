using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using CreateCohortResponse = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.CreateCohortResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingAddDraftApprenticeshipOrRoute
{
    [Test, MoqAutoData]
    public async Task WithoutEditThenReturnsRedirect(
        CreateCohortApimRequest request,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient)
    {
        var viewModel = new ApprenticeViewModel();
        viewModel.AddApprenticeshipCacheKey = cacheModel.CacheKey;
        var autoFixture = new Fixture();

        var createCohortResponse = autoFixture.Create<CreateCohortResponse>();
        var getDraftApprenticeshipsResponse = autoFixture.Build<GetDraftApprenticeshipsResponse>()
            .With(x => x.DraftApprenticeships, new DraftApprenticeshipDto[1] { autoFixture.Create<DraftApprenticeshipDto>() })
            .Create();

        cacheStorageService
           .Setup(x => x.DeleteFromCache(cacheModel.CacheKey))
           .Returns(Task.CompletedTask);

        approvalsApiClient.Setup(x => x.CreateCohort(It.IsAny<CreateCohortApimRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createCohortResponse);
        commitmentsApiClient.Setup(x => x.GetDraftApprenticeships(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getDraftApprenticeshipsResponse);

        mockMapper.Setup(mapper => mapper.Map<CreateCohortApimRequest>(viewModel))
            .ReturnsAsync(request);

        var controller = new CohortController(
            commitmentsApiClient.Object,
            Mock.Of<ILogger<CohortController>>(),
            Mock.Of<ILinkGenerator>(),
            mockMapper.Object,
            Mock.Of<IEncodingService>(),
            approvalsApiClient.Object,
            cacheStorageService.Object);

        var result = await controller.AddDraftApprenticeshipOrRoute("", "", viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("SelectOption");
        result.ControllerName.Should().Be("DraftApprenticeship");
    }

    [Test, MoqAutoData]
    public async Task WithEditCourseThenReturnsRedirect(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var viewModel = new ApprenticeViewModel
        {
            AddApprenticeshipCacheKey = cacheModel.CacheKey,
            AccountHashedId = cacheModel.AccountHashedId
        };

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.CacheKey))
          .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        var controller = new CohortController(
         Mock.Of<ICommitmentsApiClient>(),
         Mock.Of<ILogger<CohortController>>(),
         Mock.Of<ILinkGenerator>(),
         Mock.Of<IModelMapper>(),
         Mock.Of<IEncodingService>(),
         Mock.Of<IApprovalsApiClient>(),
         cacheStorageService.Object);

        var result = await controller.AddDraftApprenticeshipOrRoute("Edit", "", viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("SelectCourse");
        result.RouteValues["AccountHashedId"].Should().Be(viewModel.AccountHashedId);
        result.RouteValues["CacheKey"].Should().Be(viewModel.AddApprenticeshipCacheKey);
    }

    [Test, MoqAutoData]
    public async Task WithEditDeliveryModelThenReturnsRedirect(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        var viewModel = new ApprenticeViewModel
        {
            AddApprenticeshipCacheKey = cacheModel.CacheKey,
            AccountHashedId = cacheModel.AccountHashedId
        };

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.CacheKey))
          .ReturnsAsync(cacheModel);

        cacheStorageService
            .Setup(x => x.SaveToCache(It.IsAny<Guid>(), It.IsAny<AddApprenticeshipCacheModel>(), 1))
            .Returns(Task.CompletedTask);

        var controller = new CohortController(
         Mock.Of<ICommitmentsApiClient>(),
         Mock.Of<ILogger<CohortController>>(),
         Mock.Of<ILinkGenerator>(),
         Mock.Of<IModelMapper>(),
         Mock.Of<IEncodingService>(),
         Mock.Of<IApprovalsApiClient>(),
         cacheStorageService.Object);
        var result = await controller.AddDraftApprenticeshipOrRoute("", "Edit", viewModel) as RedirectToActionResult;

        result.Should().NotBeNull();
        result.ActionName.Should().Be("SelectDeliveryModel");
        result.RouteValues["AccountHashedId"].Should().Be(viewModel.AccountHashedId);
        result.RouteValues["CacheKey"].Should().Be(viewModel.AddApprenticeshipCacheKey);
    }
}