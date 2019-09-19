using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class EditDraftApprenticeshipTests
    {
        [Test]
        public async Task GetEditDraftApprenticeship_ValidModel_ShouldReturnViewModel()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture().WithDraftApprenticeship().WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(fixtures.EditDraftApprenticeshipRequest);
           
            var model = result.VerifyReturnsViewModel().WithModel<EditDraftApprenticeshipViewModel>();
            Assert.AreSame(model, fixtures.EditDraftApprenticeshipViewModel);
        }

        [Test]
        public async Task GetEditDraftApprenticeship_ValidModelButCohortIsWithProvider_ShouldRedirectUserToViewDetails()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture().WithDraftApprenticeship();
            fixtures.WithOtherParty();
            fixtures.WithCohort(new CohortDetails{CohortId = fixtures.CohortId}).WithViewApprenticeLink("XYZ");

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

        [Test]
        public async Task PostEditDraftApprenticeship_WithValidModel__WithEnhancedApproval_ShouldSaveDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixtures = new EditDraftApprenticeshipTestsFixture()
                .WithEnhancedApproval()
                .WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            fixtures.CommitmentsServiceMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>()), Times.Once);
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
        }
    }

    public class EditDraftApprenticeshipTestsFixture
    {
        public EditDraftApprenticeshipTestsFixture()
        {
            CommitmentsServiceMock = new Mock<ICommitmentsService>();
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            EditDraftApprenticeshipViewModel = new EditDraftApprenticeshipViewModel
            {
                CohortId = CohortId
            };

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
            ModelMapperMock.Setup(x => x.Map<EditDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipRequest>()))
                .ReturnsAsync(EditDraftApprenticeshipViewModel);

            AuthorizationServiceMock = new Mock<IAuthorizationService>();
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(false);

            Sut = new DraftApprenticeshipController(CommitmentsServiceMock.Object,
                LinkGeneratorMock.Object,
                ModelMapperMock.Object,
                Mock.Of<ICommitmentsApiClient>(),
                AuthorizationServiceMock.Object);
        }

        public Mock<ICommitmentsService> CommitmentsServiceMock { get; }
        public Mock<IModelMapper> ModelMapperMock { get; }
        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
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
        public EditDraftApprenticeshipViewModel EditDraftApprenticeshipViewModel;

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

        public EditDraftApprenticeshipTestsFixture WithEnhancedApproval()
        {
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(true);
            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithModelStateError()
        {
            Sut.ModelState.AddModelError("AKey", "Some Error");
            return this;
        }

        public EditDraftApprenticeshipTestsFixture WithOtherParty()
        {
            ModelMapperMock.Setup(x => x.Map<EditDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipRequest>()))
                .Throws(new CohortEmployerUpdateDeniedException("Cohort With Other Party"));

            return this;
        }
    }
}