using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
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
                .WithCohortWithEmployer()
                .WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference, DraftApprenticeshipId = fixtures.DraftApprenticeshipId });

            fixtures.CommitmentsApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
        }

        [Test]
        public async Task PostDetails_WithValidModel__WithEnhancedApproval_ShouldSaveDraftApprenticeshipAndRedirectToCohortDetailsV2Page()
        {
            var fixtures = new DetailsTestFixture()
                .WithCohort();

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
            CohortDetails = new GetCohortResponse
            {
                CohortId = CohortId,
                ProviderName = "ProviderName",
                WithParty = Party.Employer
            };
            EditDraftApprenticeshipDetails = new GetDraftApprenticeshipResponse
            {
                Id = DraftApprenticeshipId
            };
            ApiErrors = new List<ErrorDetail> { new ErrorDetail("Field1", "Message1") };

            ModelMapperMock = new Mock<IModelMapper>();
            ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<DetailsRequest>()))
                .ReturnsAsync(ViewModel.Object);

            AuthorizationServiceMock = new Mock<IAuthorizationService>();
            AuthorizationServiceMock.Setup(x => x.IsAuthorized(EmployerFeature.EnhancedApproval)).Returns(false);

            Sut = new DraftApprenticeshipController(
                ModelMapperMock.Object,
                CommitmentsApiClientMock.Object,
                AuthorizationServiceMock.Object);
        }

        public Mock<IModelMapper> ModelMapperMock { get; }
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public GetCohortResponse CohortDetails { get; private set; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public string DraftApprenticeshipHashedId => "DAHID";
        public GetDraftApprenticeshipResponse EditDraftApprenticeshipDetails { get; private set; }
        public DraftApprenticeshipController Sut { get; private set; }
        public List<ErrorDetail> ApiErrors { get; private set; }
        public DetailsRequest DetailsRequest;
        public Mock<IDraftApprenticeshipViewModel> ViewModel;

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

        public DetailsTestFixture WithDraftApprenticeship(GetDraftApprenticeshipResponse details = null)
        {
            var returnValue = details ?? EditDraftApprenticeshipDetails;

            CommitmentsApiClientMock
                .Setup(cs => cs.GetDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);

            return this;
        }

        public DetailsTestFixture WithCohort(GetCohortResponse cohortDetails = null)
        {
            var returnValue = cohortDetails ?? CohortDetails;

            CommitmentsApiClientMock
                .Setup(cs => cs.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);

            return this;
        }
    }
}