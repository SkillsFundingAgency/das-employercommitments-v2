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
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

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

            var result = await fixtures.Sut.EditDraftApprenticeship(string.Empty, string.Empty, new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference });

            fixtures.OuterApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipApimRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirect();
        }

        [Test]
        public async Task PostDetails_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToSelectOptionPage()
        {
            var fixtures = new DetailsTestFixture()
                .WithCohort();

            var result = await fixtures.Sut.EditDraftApprenticeship(string.Empty, string.Empty, new EditDraftApprenticeshipViewModel { AccountHashedId = fixtures.AccountHashedId, CohortId = fixtures.CohortId, CohortReference = fixtures.CohortReference });

            fixtures.OuterApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(fixtures.CohortId, fixtures.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipApimRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var redirect = result.VerifyReturnsRedirectToActionResult();
            Assert.AreEqual("SelectOption", redirect.ActionName);
        }

        [Test]
        public async Task GetViewDetails_Cohort_With_Employer_ShouldReturnViewPage()
        {
            var fixtures = new DetailsTestFixture()
                .WithEmployerView();

            var result = await fixtures.Sut.ViewDetails(fixtures.DetailsRequest);
            var viewResult = result.VerifyReturnsViewModel();
            Assert.AreEqual("View", viewResult.ViewName);
        }
    }

    public class DetailsTestFixture
    {
        public DetailsTestFixture()
        {
            var viewDraftApprenticeshipViewModel = new ViewDraftApprenticeshipViewModel();
            ViewModel = new Mock<IDraftApprenticeshipViewModel>();
            CommitmentsApiClientMock = new Mock<ICommitmentsApiClient>();
            OuterApiClientMock = new Mock<IApprovalsApiClient>();
            ReturnedDraftApprenticeshipId = DraftApprenticeshipId;

            DetailsRequest = new DetailsRequest
            {
                AccountHashedId = AccountHashedId,
                CohortReference = CohortReference,
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
            ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.Is<DetailsRequest>(dr => dr.DraftApprenticeshipHashedId == DraftApprenticeshipHashedId)))
                .ReturnsAsync(ViewModel.Object);

            ModelMapperMock.Setup(x => x.Map<ViewDraftApprenticeshipViewModel>(It.Is<DetailsRequest>(dr => dr.DraftApprenticeshipHashedId == DraftApprenticeshipHashedId)))
                .ReturnsAsync(viewDraftApprenticeshipViewModel);

            AuthorizationServiceMock = new Mock<IAuthorizationService>();

            EncodigService = new Mock<IEncodingService>();

            EncodigService.Setup(v => v.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns(ReturnedDraftApprenticeshipId);

            Sut = new DraftApprenticeshipController(
                ModelMapperMock.Object,
                CommitmentsApiClientMock.Object,
                EncodigService.Object,
                OuterApiClientMock.Object);
        }

        public Mock<IModelMapper> ModelMapperMock { get; }
        public Mock<IAuthorizationService> AuthorizationServiceMock { get; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
        public Mock<IApprovalsApiClient> OuterApiClientMock { get; }
        public GetCohortResponse CohortDetails { get; private set; }
        public string AccountHashedId => "ACHID";
        public long CohortId => 1;
        public string CohortReference => "CHREF";
        public long DraftApprenticeshipId => 99;
        public long ViewDraftApprenticeshipId => 77;
        public long ReturnedDraftApprenticeshipId { get; set; }
        public string DraftApprenticeshipHashedId => "DAHID";
        public GetDraftApprenticeshipResponse EditDraftApprenticeshipDetails { get; private set; }
        public DraftApprenticeshipController Sut { get; private set; }
        public List<ErrorDetail> ApiErrors { get; private set; }
        public DetailsRequest DetailsRequest;
        public Mock<IDraftApprenticeshipViewModel> ViewModel;
        public Mock<IEncodingService> EncodigService { get; set; }

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

        internal DetailsTestFixture WithEmployerView()
        {
            ReturnedDraftApprenticeshipId = ViewDraftApprenticeshipId;
            return this;
        }
    }
}