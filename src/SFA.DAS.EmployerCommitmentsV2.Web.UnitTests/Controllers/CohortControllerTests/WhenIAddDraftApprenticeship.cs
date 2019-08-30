using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class CreateCohortWithDraftApprenticeshipControllerTests
    {
        [Test]
        public void Constructor_WithAllParams_ShouldNotThrowException()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures();

            fixtures.CreateController();

            Assert.Pass("Created controller without exception");
        }

        [Test]
        public async Task GetAddDraftApprenticeship_ValidModel_ShouldReturnViewModel()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForGetRequest()
                .WithTrainingProvider();

            var result = await fixtures.CheckGet();

            result
                .VerifyReturnsViewModel()
                .WithModel<ApprenticeViewModel>();
        }

        [Test]
        public async Task PostAddDraftApprenticeship_WithInvalidRequest_ShouldReturnRedirectToGet()
        {
            const string reviewCohortUrl = "https://www.reviewmycohort.gov.uk";

            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                                .ForPostRequest()
                                .WithCreatedCohort("ABC123", 123)
                                .WithReviewCohortLink(reviewCohortUrl);

            var result = await fixtures.CheckPost();

            result.VerifyReturnsRedirect().WithUrl(reviewCohortUrl);
        }

        [Test]
        public async Task PostAddDraftApprenticeship_WithValidModel_ShouldSaveCohort()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForPostRequest()
                .WithCreatedCohort("ABC123", 123)
                .WithReviewCohortLink("someurl")
                .WithTrainingProvider();

            await fixtures.CheckPost();

            fixtures.CommitmentsServiceMock.Verify(cs => cs.CreateCohort(It.IsAny<CreateCohortRequest>()), Times.Once);
        }
    }

    public class CreateCohortWithDraftApprenticeshipControllerTestFixtures
    {
        public CreateCohortWithDraftApprenticeshipControllerTestFixtures()
        {
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            RequestMapper = new AddDraftApprenticeshipToCreateCohortRequestMapper();
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<ApprenticeViewModel>(It.IsAny<ApprenticeRequest>()))
                .ReturnsAsync(new ApprenticeViewModel());
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; } 
        public ICommitmentsService CommitmentsService => CommitmentsServiceMock.Object;

        public IMapper<AddDraftApprenticeshipViewModel, CreateCohortRequest> RequestMapper { get; }

        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

        public Mock<IModelMapper> ModelMapperMock { get; set; }
        public IModelMapper ModelMapper => ModelMapperMock.Object;

        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClientMock { get; }

        public ApprenticeRequest GetRequest { get; private set; }
        public ApprenticeViewModel PostRequest { get; private set; }
        
        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public ICommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForGetRequest()
        {
            GetRequest = new ApprenticeRequest {ProviderId = 1};
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForPostRequest()
        {
            PostRequest = new ApprenticeViewModel {ProviderId = 1};
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithReviewCohortLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithCreatedCohort(string cohortReference, long  cohortId)
        {
            CommitmentsServiceMock
                .Setup(cs => cs.CreateCohort(It.IsAny<CreateCohortRequest>()))
                .ReturnsAsync(new CreateCohortResponse {CohortReference = cohortReference, CohortId = cohortId});

            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithTrainingProvider()
        {
            var response = new GetProviderResponse {Name = "Name", ProviderId = 1};

            CommitmentsApiClientMock.Setup(p => p.GetProvider(1, CancellationToken.None))
                .ReturnsAsync(response);

            return this;
        }

        public CohortController CreateController()
        {
            var controller = new CohortController(
                CommitmentsApiClient,
                Mock.Of<ILogger<CohortController>>(),
                CommitmentsService,
                LinkGenerator,
                ModelMapper
            );
            return controller;
        }

        public Task<IActionResult> CheckGet()
        {
            var controller = CreateController();

            return controller.Apprentice(GetRequest);
        }

        public Task<IActionResult> CheckPost()
        {
            var controller = CreateController();

            return controller.Apprentice(PostRequest);
        }
    }
}