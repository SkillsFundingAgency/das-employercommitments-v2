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
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;

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
                .WithModel<AddDraftApprenticeshipViewModel>();
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
            UrlSelectorServiceMock = new Mock<IUrlSelectorService>();
            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<AddDraftApprenticeshipViewModel>(It.IsAny<CreateCohortWithDraftApprenticeshipRequest>()))
                .ReturnsAsync(new AddDraftApprenticeshipViewModel());
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; } 
        public ICommitmentsService CommitmentsService => CommitmentsServiceMock.Object;

        public IMapper<AddDraftApprenticeshipViewModel, CreateCohortRequest> RequestMapper { get; }

        public Mock<IUrlSelectorService> UrlSelectorServiceMock { get; }
        public IUrlSelectorService UrlSelectorService => UrlSelectorServiceMock.Object;

        public Mock<IModelMapper> ModelMapperMock { get; set; }
        public IModelMapper ModelMapper => ModelMapperMock.Object;

        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClientMock { get; }

        public CreateCohortWithDraftApprenticeshipRequest GetRequest { get; private set; }
        public AddDraftApprenticeshipViewModel PostRequest { get; private set; }
        
        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public ICommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForGetRequest()
        {
            GetRequest = new CreateCohortWithDraftApprenticeshipRequest {ProviderId = 1};
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForPostRequest()
        {
            PostRequest = new AddDraftApprenticeshipViewModel();
            PostRequest.ProviderId = 1;
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithReviewCohortLink(string url)
        {
            UrlSelectorServiceMock
                .Setup(lg => lg.RedirectToCohortDetails(
                    It.IsAny<IUrlHelper>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new RedirectResult(url));

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
                ModelMapper,
                UrlSelectorService
            );
            return controller;
        }

        public Task<IActionResult> CheckGet()
        {
            var controller = CreateController();

            return controller.AddDraftApprenticeship(GetRequest);
        }

        public Task<IActionResult> CheckPost()
        {
            var controller = CreateController();

            return controller.AddDraftApprenticeship(PostRequest);
        }
    }
}