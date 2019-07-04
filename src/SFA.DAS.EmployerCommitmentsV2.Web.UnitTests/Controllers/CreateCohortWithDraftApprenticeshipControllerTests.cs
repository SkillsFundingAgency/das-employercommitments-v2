using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers
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
                                .ForGetRequest();

            var result = await fixtures.CheckGet();

            result
                .VerifyReturnsViewModel()
                .WithModel<AddDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task GetAddDraftApprenticeship_WithValidModel_ShouldSeeAllCourses()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForGetRequest();

            await fixtures.CheckGet();

            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetStandardTrainingProgrammes(), Times.Never);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetFrameworkTrainingProgrammes(), Times.Never);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetAllTrainingProgrammes(), Times.Once);
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
        public async Task PostAddDraftApprenticeship_WithInvalidModel_ShouldReturnViewModel()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForPostRequest()
                .WithInvalidModel();

            var result = await fixtures.CheckPost();

            result.VerifyReturnsViewModel().WithModel<AddDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task PostAddDraftApprenticeship_WithInvalidModel_ShouldReturnViewModelWithCoursesSet()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForPostRequest()
                .WithInvalidModel()
                .WithCourses("001", "002");

            var result = await fixtures.CheckPost();

            var model = result.VerifyReturnsViewModel().WithModel<AddDraftApprenticeshipViewModel>();

            Assert.IsNotNull(model, "Model is null");
            Assert.IsNotEmpty(model.Courses, "Courses were empty");
        }

        [Test]
        public async Task PostAddDraftApprenticeship_WithValidModel_ShouldSaveCohort()
        {
            var fixtures = new CreateCohortWithDraftApprenticeshipControllerTestFixtures()
                .ForPostRequest()
                .WithCreatedCohort("ABC123", 123)
                .WithReviewCohortLink("someurl");

            await fixtures.CheckPost();

            fixtures.CommitmentsServiceMock.Verify(cs => cs.CreateCohort(It.IsAny<CreateCohortRequest>()), Times.Once);
        }
    }

    public class CreateCohortWithDraftApprenticeshipControllerTestFixtures
    {
        private bool _setModelToInvalid;

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures()
        {
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            RequestMapper = new AddDraftApprenticeshipRequestMapper();
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; } 
        public ICommitmentsService CommitmentsService => CommitmentsServiceMock.Object;

        public IMapper<AddDraftApprenticeshipViewModel, CreateCohortRequest> RequestMapper { get; }

        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClientMock { get; }
        public ITrainingProgrammeApiClient TrainingProgrammeApiClient => TrainingProgrammeApiClientMock.Object;

        public CreateCohortWithDraftApprenticeshipRequest GetRequest { get; private set; }
        public AddDraftApprenticeshipViewModel PostRequest { get; private set; }
        
        public Mock<CommitmentsApiClient> CommitmentsApiClientMock { get; }
        public CommitmentsApiClient CommitmentsApiClient => CommitmentsApiClientMock.Object;

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForGetRequest()
        {
            GetRequest = new CreateCohortWithDraftApprenticeshipRequest();
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures ForPostRequest()
        {
            PostRequest = new AddDraftApprenticeshipViewModel();
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithReviewCohortLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CohortDetails(It.IsAny<string>(), It.IsAny<string>()))
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

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithInvalidModel()
        {
            _setModelToInvalid = true;
            return this;
        }

        public CreateCohortWithDraftApprenticeshipControllerTestFixtures WithCourses(params string[] courseCodes)
        {
            TrainingProgrammeApiClientMock
                .Setup(tp => tp.GetAllTrainingProgrammes())
                .ReturnsAsync(courseCodes.Select(cc =>
                {
                    var trainingProgramMock = new Mock<ITrainingProgramme>();
                    trainingProgramMock.Setup(tpm => tpm.Id).Returns(cc);
                    return trainingProgramMock.Object;
                }).ToList());

            return this;
        }

        public CreateCohortWithDraftApprenticeshipController CreateController()
        {
            var controller = new CreateCohortWithDraftApprenticeshipController(
                CommitmentsService, 
                RequestMapper, 
                LinkGenerator, 
                TrainingProgrammeApiClient,
                CommitmentsApiClient);

            if (_setModelToInvalid)
            {
                controller.ModelState.AddModelError("AKey", "Some Error");
            }

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