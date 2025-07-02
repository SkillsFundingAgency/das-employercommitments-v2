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
using CreateCohortResponse = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.CreateCohortResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class CreateCohortWithDraftApprenticeshipControllerTests
{
    [Test]
    public void Constructor_WithAllParams_ShouldNotThrowException()
    {
        var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures();

        Action act = () => fixtures.CreateController();

        act.Should().NotThrow();
    }

    [Test]
    public async Task PostAddDraftApprenticeship_WithValidModel_WithoutCourseSelected_ShouldRedirectToCohortDetailsV2()
    {
        var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
            .ForPostRequest()
            .SetupEncodingService()
            .WithCreatedCohort("ABC123", 123)
            .WithDraftApprenticeship(123, withCourseSelected: false);

        var result = await fixtures.CheckPost();

        result.VerifyReturnsRedirectToActionResult().WithActionName("CohortDetails");
    }

    [Test]
    public async Task PostAddDraftApprenticeship_WithValidModel_WithCourseSelected_ShouldRedirectToSelectOptions()
    {
        var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
            .ForPostRequest()
            .SetupEncodingService()
            .WithCreatedCohort("ABC123", 123)
            .WithDraftApprenticeship(123, withCourseSelected: true);

        var result = await fixtures.CheckPost();

        result.VerifyReturnsRedirectToActionResult().WithActionName("SelectOption");
    }

    [Test]
    public async Task PostAddDraftApprenticeship_WithValidModel_ShouldSaveCohort()
    {
        var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
            .ForPostRequest()
            .SetupEncodingService()
            .WithCreatedCohort("ABC123", 123)
            .WithDraftApprenticeship(123)
            .WithTrainingProvider();

        await fixtures.CheckPost();

        fixtures.ApprovalsApiClientMock.Verify(cs => cs.CreateCohort(It.IsAny<CreateCohortApimRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class CreateCohortWithDraftApprenticeshipControllerTestFixtures
{
    public CreateCohortWithDraftApprenticeshipControllerTestFixtures()
    {
        LinkGeneratorMock = new Mock<ILinkGenerator>();
        CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
        ModelMapperMock = new Mock<IModelMapper>();
        ModelMapperMock.Setup(x => x.Map<ApprenticeViewModel>(It.IsAny<ApprenticeRequest>()))
            .ReturnsAsync(new ApprenticeViewModel());

        CacheStorageServiceMock = new Mock<ICacheStorageService>();
        CacheStorageServiceMock.Setup(x => x.DeleteFromCache(CacheKey))
           .Returns(Task.CompletedTask);
        EncodingServiceMock = new Mock<IEncodingService>();
        ApprovalsApiClientMock = new Mock<IApprovalsApiClient>();
    }
    private static Guid CacheKey => Guid.NewGuid();

    public Mock<ILinkGenerator> LinkGeneratorMock { get; }
    public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

    public Mock<IModelMapper> ModelMapperMock { get; set; }
    public IModelMapper ModelMapper => ModelMapperMock.Object;

    public Mock<IEncodingService> EncodingServiceMock { get; set; }
    public IEncodingService EncodingService => EncodingServiceMock.Object;

    public Mock<ICacheStorageService> CacheStorageServiceMock { get; set; }
    public ICacheStorageService CacheStorageService => CacheStorageServiceMock.Object;

    public ApprenticeViewModel PostRequest { get; private set; }
    public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
    public Mock<IApprovalsApiClient> ApprovalsApiClientMock { get; }
    public ICommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;
    public IApprovalsApiClient ApprovalsApiClient => ApprovalsApiClientMock.Object;
   
    public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForPostRequest()
    {
        PostRequest = new ApprenticeViewModel { ProviderId = 1, ApprenticeshipSessionKey = CacheKey };
        return this;
    }

    public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithCreatedCohort(string cohortReference, long cohortId)
    {
        ApprovalsApiClientMock
            .Setup(cs => cs.CreateCohort(It.IsAny<CreateCohortApimRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateCohortResponse { CohortReference = cohortReference, CohortId = cohortId });

        return this;
    }

    public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithTrainingProvider()
    {
        var response = new GetProviderResponse { Name = "Name", ProviderId = 1 };

        CommitmentsApiClientMock.Setup(p => p.GetProvider(1, CancellationToken.None))
            .ReturnsAsync(response);

        return this;
    }

    public CreateCohortWithDraftApprenticeshipControllerTestFixtures SetupEncodingService()
    {
        EncodingServiceMock
            .Setup(e => e.Encode(It.IsAny<long>(), EncodingType.ApprenticeshipId))
            .Returns("APP123");

        return this;
    }

    public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithDraftApprenticeship(long cohortId, bool withCourseSelected = false)
    {
        CommitmentsApiClientMock
            .Setup(c => c.GetDraftApprenticeships(cohortId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetDraftApprenticeshipsResponse
            {
                DraftApprenticeships = new List<DraftApprenticeshipDto>
                {
                    new()
                    {
                        Id = 123456,
                        CourseCode = withCourseSelected ? "ST0001" : null
                    }
                }
            });

        return this;
    }

    public CohortController CreateController()
    {
        var controller = new CohortController(
            CommitmentsApiClient,
            Mock.Of<ILogger<CohortController>>(),
            LinkGenerator,
            ModelMapper,
            Mock.Of<IEncodingService>(),
            ApprovalsApiClient,
            CacheStorageService
        );
        return controller;
    }

    public async Task<IActionResult> CheckPost()
    {
        var controller = CreateController();

        return await controller.AddDraftApprenticeshipOrRoute(string.Empty, string.Empty, PostRequest);
    }
}