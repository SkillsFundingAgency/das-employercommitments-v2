using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class DraftApprenticeshipControllerTests
    {
        [Test]
        public async Task GetEditDraftApprenticeship_ValidModel_ShouldReturnBadRequestIfModelStateIsNotValid()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithModelStateError();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipRequest());

            result.VerifyReturnsBadRequestObject();
        }

        [Test]
        public async Task GetEditDraftApprenticeship_ValidModel_ShouldReturnViewModel()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithCourses().WithDraftApprenticeship().WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipRequest { CohortId = fixtures.CohortId, DraftApprenticeshipId = fixtures.DraftApprenticeshipId});

            fixtures.CommitmentsServiceMock.Verify(x => x.GetCohortDetail(fixtures.CohortId));
            fixtures.CommitmentsServiceMock.Verify(x => x.GetDraftApprenticeshipForCohort(fixtures.CohortId, fixtures.DraftApprenticeshipId));
            result.VerifyReturnsViewModel().WithModel<EditDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task GetEditDraftApprenticeship_WithValidModelAndTransferCohort_ShouldSeeStandardCourses()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithStandardCourses().WithDraftApprenticeship().WithTransferCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipRequest { CohortId = fixtures.CohortId, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetStandardTrainingProgrammes(), Times.Once);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetAllTrainingProgrammes(), Times.Never);
        }

        [Test]
        public async Task GetEditDraftApprenticeship_ValidModel_ShouldReturnViewModelWithProviderName()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithCourses().WithDraftApprenticeship().WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipRequest { CohortId = fixtures.CohortId, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            var model = result.VerifyReturnsViewModel().WithModel<EditDraftApprenticeshipViewModel>();
            Assert.AreEqual("ProviderName", model.ProviderName);
        }

        [Test]
        public async Task PostEditDraftApprenticeship_WithInvalidModel_ShouldReturnTheViewModelAndAddProviderNameAndCourses()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithModelStateError().WithCourses("XXX", "YYYY").WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel { DraftApprenticeshipId = fixtures.DraftApprenticeshipId, CohortId = fixtures.CohortId, FirstName = "First", LastName = "Last"});

            var model = result.VerifyReturnsViewModel().WithModel<EditDraftApprenticeshipViewModel>();
            Assert.AreEqual("First", model.FirstName);
            Assert.AreEqual("Last", model.LastName);
            Assert.AreEqual(2, model.Courses.Count);
            Assert.AreEqual("ProviderName", model.ProviderName);
        }

        [Test]
        public async Task PostEditDraftApprenticeship_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToCohortPage()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures()
                .WithCohort()
                .WithCohortLink("cohortPage");

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel{ AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId});

            fixtures.CommitmentsServiceMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("cohortPage", redirect.Url);
        }
    }

    public class DraftApprenticeshipControllerTestFixtures
    {
        private long draftApprentice_setModelToInvalid;

        public DraftApprenticeshipControllerTestFixtures()
        {
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            ToViewModelMapper = new EditDraftApprenticeshipDetailsToViewModelMapper();
            ToApiRequestMapper = new EditDraftApprenticeshipToUpdateRequestMapper();
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();

            CohortDetails = new CohortDetails { CohortId = CohortId, HashedCohortId = CohortReference, IsFundedByTransfer = false, ProviderName = "ProviderName"};
            EditDraftApprenticeshipDetails = new EditDraftApprenticeshipDetails { CohortId = CohortId, CohortReference = CohortReference, DraftApprenticeshipId = DraftApprenticeshipId, DraftApprenticeshipHashedId = DraftApprenticeshipHashedId};

            Sut = new DraftApprenticeshipController(CommitmentsServiceMock.Object,
                ToViewModelMapper,
                ToApiRequestMapper,
                LinkGeneratorMock.Object,
                TrainingProgrammeApiClientMock.Object);
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; }
        public IMapper<EditDraftApprenticeshipDetails, EditDraftApprenticeshipViewModel> ToViewModelMapper;
        public IMapper<EditDraftApprenticeshipViewModel, UpdateDraftApprenticeshipRequest> ToApiRequestMapper;
        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClientMock { get; }
        public CohortDetails CohortDetails { get; private set; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public string DraftApprenticeshipHashedId => "DAHID";
        public EditDraftApprenticeshipDetails EditDraftApprenticeshipDetails { get; private set; }
        public DraftApprenticeshipController Sut { get; private set; }

        public DraftApprenticeshipControllerTestFixtures WithCohortLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CohortDetails(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public DraftApprenticeshipControllerTestFixtures WithDraftApprenticeship(EditDraftApprenticeshipDetails details = null)
        {
            var returnValue = details ?? EditDraftApprenticeshipDetails;

            CommitmentsServiceMock
                .Setup(cs => cs.GetDraftApprenticeshipForCohort(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }

        public DraftApprenticeshipControllerTestFixtures WithCohort(CohortDetails cohortDetails = null)
        {
            var returnValue = cohortDetails ?? CohortDetails;

            CommitmentsServiceMock
                .Setup(cs => cs.GetCohortDetail(It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }
        public DraftApprenticeshipControllerTestFixtures WithTransferCohort()
        {
            var returnValue = new CohortDetails { CohortId = CohortId, HashedCohortId = CohortReference, IsFundedByTransfer =  true};

            CommitmentsServiceMock
                .Setup(cs => cs.GetCohortDetail(It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }

        public DraftApprenticeshipControllerTestFixtures WithCourses(params string[] courseCodes)
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

        public DraftApprenticeshipControllerTestFixtures WithStandardCourses(params string[] courseCodes)
        {
            TrainingProgrammeApiClientMock
                .Setup(tp => tp.GetStandardTrainingProgrammes())
                .ReturnsAsync(courseCodes.Select(cc =>
                {
                    var trainingProgramMock = new Mock<ITrainingProgramme>();
                    trainingProgramMock.Setup(tpm => tpm.Id).Returns(cc);
                    return trainingProgramMock.Object;
                }).ToList());

            return this;
        }
        public DraftApprenticeshipControllerTestFixtures WithModelStateError()
        {
            Sut.ModelState.AddModelError("AKey", "Some Error");
            return this;
        }
    }
}