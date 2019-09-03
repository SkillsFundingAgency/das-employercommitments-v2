using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class EditDraftApprenticeshipTests
    {
        [Test]
        public async Task GetEditDraftApprenticeship_ValidModel_ShouldReturnViewModel()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture().WithCourses().WithDraftApprenticeship().WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(fixtures.EditDraftApprenticeshipRequest);

            fixtures.CommitmentsServiceMock.Verify(x => x.GetCohortDetail(fixtures.CohortId));
            fixtures.CommitmentsServiceMock.Verify(x => x.GetDraftApprenticeshipForCohort(fixtures.CohortId, fixtures.DraftApprenticeshipId));
            
            var model = result.VerifyReturnsViewModel().WithModel<EditDraftApprenticeshipViewModel>();
            
            Assert.AreEqual(fixtures.AccountHashedId, model.AccountHashedId);
            Assert.AreEqual("ProviderName", model.ProviderName);
        }

        [Test]
        public async Task GetEditDraftApprenticeship_WithValidModelAndTransferCohort_ShouldSeeStandardCourses()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture().WithStandardCourses().WithDraftApprenticeship().WithTransferCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(fixtures.EditDraftApprenticeshipRequest);

            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetStandardTrainingProgrammes(), Times.Once);
            fixtures.TrainingProgrammeApiClientMock.Verify(tp => tp.GetAllTrainingProgrammes(), Times.Never);
        }

        [Test]
        public async Task GetEditDraftApprenticeship_ValidModelButCohortIsWithProvider_ShouldRedirectUserToViewDetails()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture().WithCourses().WithDraftApprenticeship();
            fixtures.WithCohort(new CohortDetails{CohortId = fixtures.CohortId, WithParty = Party.Provider}).WithViewApprenticeLink("XYZ");

            var result = await fixtures.Sut.EditDraftApprenticeship(fixtures.EditDraftApprenticeshipRequest);

            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("XYZ", redirect.Url);
            fixtures.LinkGeneratorMock.Verify(x=>x.CommitmentsLink($"accounts/{fixtures.AccountHashedId}/apprentices/{fixtures.CohortReference}/apprenticeships/{fixtures.DraftApprenticeshipHashedId}/view"));
        }

        [Test]
        public async Task PostEditDraftApprenticeship_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToCohortPage()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture()
                    .WithCohort()
                    .WithCohortLink("cohortPage");

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel{ AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId});

            fixtures.CommitmentsServiceMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("cohortPage", redirect.Url);
        }
    }

    public class EditDraftApprenticeshipTestsFixture
    {
        public EditDraftApprenticeshipTestsFixture()
        {
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            TrainingProgrammeApiClientMock = new Mock<ITrainingProgrammeApiClient>();

            EditDraftApprenticeshipRequest = new EditDraftApprenticeshipRequest
            {
                AccountHashedId = AccountHashedId, CohortId = CohortId, CohortReference = CohortReference,
                DraftApprenticeshipId = DraftApprenticeshipId, DraftApprenticeshipHashedId = DraftApprenticeshipHashedId
            };
            CohortDetails = new CohortDetails
            {
                CohortId = CohortId, HashedCohortId = CohortReference, IsFundedByTransfer = false,
                ProviderName = "ProviderName", WithParty = Party.Employer
            };
            EditDraftApprenticeshipDetails = new EditDraftApprenticeshipDetails
            {
                CohortId = CohortId, CohortReference = CohortReference, DraftApprenticeshipId = DraftApprenticeshipId,
                DraftApprenticeshipHashedId = DraftApprenticeshipHashedId
            };
            ApiErrors = new List<ErrorDetail>{new ErrorDetail("Field1", "Message1")};

            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<EditDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipDetails>()))
                .Returns(Task.FromResult(new EditDraftApprenticeshipViewModel
                {
                    CohortId = CohortId
                }));

            Sut = new DraftApprenticeshipController(CommitmentsServiceMock.Object,
                LinkGeneratorMock.Object,
                TrainingProgrammeApiClientMock.Object,
                ModelMapperMock.Object,
                Mock.Of<ICommitmentsApiClient>());
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; }
        public Mock<IModelMapper> ModelMapperMock { get; }
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
        public List<ErrorDetail> ApiErrors { get; private set; }
        public EditDraftApprenticeshipRequest EditDraftApprenticeshipRequest;

        public EditDraftApprenticeshipTestsFixture WithCohortLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithViewApprenticeLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithDraftApprenticeship(EditDraftApprenticeshipDetails details = null)
        {
            var returnValue = details ?? EditDraftApprenticeshipDetails;

            CommitmentsServiceMock
                .Setup(cs => cs.GetDraftApprenticeshipForCohort(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithUpdateDraftApprenticeshipDomainError()
        {
            CommitmentsServiceMock
                .Setup(cs => cs.UpdateDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<UpdateDraftApprenticeshipRequest>()))
                .ThrowsAsync(new CommitmentsApiModelException(ApiErrors));

            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithCohort(CohortDetails cohortDetails = null)
        {
            var returnValue = cohortDetails ?? CohortDetails;

            CommitmentsServiceMock
                .Setup(cs => cs.GetCohortDetail(It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }
        public EditDraftApprenticeshipTestsFixture WithTransferCohort()
        {
            var returnValue = new CohortDetails { CohortId = CohortId, HashedCohortId = CohortReference, IsFundedByTransfer =  true, WithParty = Party.Employer};

            CommitmentsServiceMock
                .Setup(cs => cs.GetCohortDetail(It.IsAny<long>()))
                .ReturnsAsync(returnValue);

            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithCourses(params string[] courseCodes)
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

        public EditDraftApprenticeshipTestsFixture WithStandardCourses(params string[] courseCodes)
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
        public EditDraftApprenticeshipTestsFixture WithModelStateError()
        {
            Sut.ModelState.AddModelError("AKey", "Some Error");
            return this;
        }
    }
}