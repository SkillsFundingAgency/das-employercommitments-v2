using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Requests;
using SFA.DAS.ProviderCommitments.Web.Controllers;
using SFA.DAS.ProviderUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class DraftApprenticeshipControllerTests
    {
        [Test]
        public void Constructor_WithAllParams_ShouldNotThrowException()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures();

            fixtures.CreateController();

            Assert.Pass("Created controller without exception");
        }

        [Test]
        public async Task AddDraftApprenticeship_ValidModel_ShouldReturnViewModel()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithCohortId(123);

            var result = await fixtures.CheckAddDraftApprenticeship();

            result
                .VerifyReturnsViewModel()
                .WithModel<AddDraftApprenticeshipViewModel>();
        }

        [Test]
        public async Task AddDraftApprenticeship_UsingATransferFundedCohort_ShouldOnlySeeStandardCourses()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures()
                .WithCohortId(123)
                .WithTransferFundedCohort(true);

            var result = await fixtures.CheckAddDraftApprenticeship();

            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetStandardTrainingProgrammes(), Times.Once);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetFrameworkTrainingProgrammes(), Times.Never);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetAllTrainingProgrammes(), Times.Never);
        }

        [Test]
        public async Task AddDraftApprenticeship_UsingANonTransferFundedCohort_ShouldSeeAllCourses()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures()
                .WithCohortId(123)
                .WithTransferFundedCohort(false);

            var result = await fixtures.CheckAddDraftApprenticeship();

            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetStandardTrainingProgrammes(), Times.Never);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetFrameworkTrainingProgrammes(), Times.Never);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetAllTrainingProgrammes(), Times.Once);
        }

        [Test]
        [Ignore("This requires some setup from CV-190 branch")]
        public async Task AddDraftApprenticeship_InvalidModel_ShouldReturnBadRequest()
        {
            var fixtures = new DraftApprenticeshipControllerTestFixtures().WithCohortId(null);

            var result = await fixtures.CheckAddDraftApprenticeship();

            result.VerifyReturnsRedirect().WithUrl("add");
        }
    }

    public class DraftApprenticeshipControllerTestFixtures
    {
        public DraftApprenticeshipControllerTestFixtures()
        {
            Request = new ReservationsAddDraftApprenticeshipRequest();
            CohortDetails = new CohortDetails();

            CommitmentsServiceMock = new Mock<ICommitmentsService>();

            RequestMapper = new AddDraftApprenticeshipRequestMapper();

            LinkGeneratorMock = new Mock<ILinkGenerator>();

            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();
            CommitmentsServiceMock
                .Setup(cs => cs.GetCohortDetail(It.IsAny<long>()))
                .ReturnsAsync(CohortDetails);
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; } 
        public ICommitmentsService CommitmentsService => CommitmentsServiceMock.Object;

        public IMapper<AddDraftApprenticeshipViewModel, AddDraftApprenticeshipRequest> RequestMapper { get; }

        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public ILinkGenerator LinkGenerator => LinkGeneratorMock.Object;

        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClientMock { get; }
        public ITrainingProgrammeApiClient TrainingProgrammeApiClient => TrainingProgrammeApiClientMock.Object;

        public ReservationsAddDraftApprenticeshipRequest Request { get; }
        public CohortDetails CohortDetails { get; }

        public DraftApprenticeshipControllerTestFixtures WithCohortId(long? cohortId)
        {
            Request.CohortId = cohortId;
            CohortDetails.CohortId = cohortId ?? 0;
            return this;
        }

        public DraftApprenticeshipControllerTestFixtures WithTransferFundedCohort(bool isFundedByTransfer)
        {
            CohortDetails.IsFundedByTransfer = isFundedByTransfer;
            return this;
        }

        public DraftApprenticeshipController CreateController()
        {
            return new DraftApprenticeshipController(CommitmentsService, RequestMapper, LinkGenerator, TrainingProgrammeApiClient);
        }

        public Task<IActionResult> CheckAddDraftApprenticeship()
        {
            var controller = CreateController();

            return controller.AddDraftApprenticeship(Request);
        }
    }
}