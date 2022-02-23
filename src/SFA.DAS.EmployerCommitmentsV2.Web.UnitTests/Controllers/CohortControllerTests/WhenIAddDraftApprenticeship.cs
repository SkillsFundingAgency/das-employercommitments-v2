using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types.Dtos;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

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
        public async Task PostAddDraftApprenticeship_WithValidModel_WithEnhancedApproval_WithoutCourseSelected_ShouldRedirectToCohortDetailsV2()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForPostRequest()
                .SetupEncodingService()
                .WithCreatedCohort("ABC123", 123)
                .WithDraftApprenticeship(123, withCourseSelected : false);

            var result = await fixtures.CheckPost();

            result.VerifyReturnsRedirectToActionResult().WithActionName("Details");
        }

        [Test]
        public async Task PostAddDraftApprenticeship_WithValidModel_WithEnhancedApproval_WithCourseSelected_ShouldRedirectToSelectOptions()
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
            
            fixtures.CommitmentsApiClientMock.Verify(cs => cs.CreateCohort(It.IsAny<CreateCohortRequest>(), It.IsAny<CancellationToken>()), Times.Once);
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
            AuthorizationServiceMock = new Mock<IAuthorizationService>();
            EncodingServiceMock = new Mock<IEncodingService>();
        }

        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

        public Mock<IModelMapper> ModelMapperMock { get; set; }
        public IModelMapper ModelMapper => ModelMapperMock.Object;

        public Mock<IAuthorizationService> AuthorizationServiceMock { get; set; }
        public IAuthorizationService AuthorizationService => AuthorizationServiceMock.Object;
        public Mock<IEncodingService> EncodingServiceMock { get; set; }
        public IEncodingService EncodingService => EncodingServiceMock.Object;
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

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithCreatedCohort(string cohortReference, long  cohortId)
        {
            CommitmentsApiClientMock
                .Setup(cs => cs.CreateCohort(It.IsAny<CreateCohortRequest>(), It.IsAny<CancellationToken>()))
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
                        new DraftApprenticeshipDto
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
                AuthorizationService,
                Mock.Of<IEncodingService>()
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