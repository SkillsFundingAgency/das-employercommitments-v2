using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class DetailsTests
    {
        [Test]
        public async Task GetDetails_ShouldReturnViewModel()
        {
            var fixtures = new DetailsTestFixture();

            var result = await fixtures.Sut.Details(fixtures.DetailsRequest);
            var viewResult = result.VerifyReturnsViewModel();
            Assert.AreEqual(fixtures.ViewModel.Object, viewResult.Model);
        }

        [Test]
        public async Task GetDetails_Cohort_With_Employer_ShouldReturnEditPage()
        {
            var fixtures = new DetailsTestFixture()
                .WithCohortWithEmployer();

            var result = await fixtures.Sut.Details(fixtures.DetailsRequest);
            var viewResult = result.VerifyReturnsViewModel();
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [Test]
        public async Task GetDetails_Cohort_With_OtherParty_ShouldReturnViewPage()
        {
            var fixtures = new DetailsTestFixture()
                .WithCohortWithOtherParty();

            var result = await fixtures.Sut.Details(fixtures.DetailsRequest);
            var viewResult = result.VerifyReturnsViewModel();
            Assert.AreEqual("View", viewResult.ViewName);
        }

        [Test]
        public async Task PostDetails_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToCohortPage()
        {
            var fixtures = new DetailsTestFixture()
                    .WithCohortLink("cohortPage");

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            fixtures.CommitmentsApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
            Assert.AreEqual("cohortPage", redirect.Url);
        }

        [Test]
        public async Task PostDetails_WithValidModel__WithEnhancedApproval_ShouldSaveDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixtures = new DetailsTestFixture()
                .WithEnhancedApproval();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            fixtures.CommitmentsApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("Details", redirect.ActionName);
        }
    }

    public class DetailsTestFixture
    {
        public DetailsTestFixture()
        {
            LinkGeneratorMock = new Mock<ILinkGenerator>();
            ViewModel = new Mock<IDraftApprenticeshipViewModel>();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();

            DetailsRequest = new DetailsRequest
            {
                AccountHashedId = AccountHashedId,
                CohortId = CohortId,
                CohortReference = CohortReference,
                DraftApprenticeshipId = DraftApprenticeshipId,
                DraftApprenticeshipHashedId = DraftApprenticeshipHashedId
            };
            CohortDetails = new CohortDetails
            {
                CohortId = CohortId,
                HashedCohortId = CohortReference,
                IsFundedByTransfer = false,
                ProviderName = "ProviderName",
                WithParty = Party.Employer
            };
            EditDraftApprenticeshipDetails = new EditDraftApprenticeshipDetails
            {
                CohortId = CohortId,
                CohortReference = CohortReference,
                DraftApprenticeshipId = DraftApprenticeshipId,
                DraftApprenticeshipHashedId = DraftApprenticeshipHashedId
            };
            ApiErrors = new List<ErrorDetail> { new ErrorDetail("Field1", "Message1") };

            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<DetailsRequest>()))
                .ReturnsAsync(ViewModel.Object);

            AuthorizationServiceMock = new Mock<IAuthorizationService>();
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(false);

            Sut = new DraftApprenticeshipController(LinkGeneratorMock.Object,
                ModelMapperMock.Object,
                CommitmentsApiClientMock.Object,
                AuthorizationServiceMock.Object);
        }

        public Mock<IModelMapper> ModelMapperMock { get; }
        public Mock<ILinkGenerator> LinkGeneratorMock { get; }
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public CohortDetails CohortDetails { get; private set; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public string DraftApprenticeshipHashedId => "DAHID";
        public EditDraftApprenticeshipDetails EditDraftApprenticeshipDetails { get; private set; }
        public DraftApprenticeshipController Sut { get; private set; }
        public List<ErrorDetail> ApiErrors { get; private set; }
        public DetailsRequest DetailsRequest;
        public Mock<IDraftApprenticeshipViewModel> ViewModel;

        public DetailsTestFixture WithCohortLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public DetailsTestFixture WithCohortWithOtherParty()
        {
            ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<DetailsRequest>()))
                .ReturnsAsync(new ViewDraftApprenticeshipViewModel());
            return this;
        }

        public DetailsTestFixture WithCohortWithEmployer()
        {
            ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<DetailsRequest>()))
                .ReturnsAsync(new EditDraftApprenticeshipViewModel());
            return this;
        }

        public DetailsTestFixture WithViewApprenticeLink(string url)
        {
            LinkGeneratorMock
                .Setup(lg => lg.CommitmentsLink(It.IsAny<string>()))
                .Returns(url);

            return this;
        }

        public DetailsTestFixture WithEnhancedApproval()
        {
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(true);
            return this;
        }

        public DetailsTestFixture WithModelStateError()
        {
            Sut.ModelState.AddModelError("AKey", "Some Error");
            return this;
        }

        public DetailsTestFixture WithOtherParty()
        {
            ModelMapperMock.Setup(x => x.Map<EditDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipRequest>()))
                .Throws(new CohortEmployerUpdateDeniedException("Cohort With Other Party"));

            return this;
        }
    }
}