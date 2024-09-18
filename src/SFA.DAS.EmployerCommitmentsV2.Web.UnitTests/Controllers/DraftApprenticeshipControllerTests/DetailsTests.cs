using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests;

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
        Assert.That(viewResult.Model, Is.EqualTo(fixtures.ViewModel.Object));
    }

    [Test]
    public async Task GetDetails_Cohort_With_Employer_ShouldReturnEditPage()
    {
        var fixtures = new DetailsTestFixture()
            .WithCohortWithEmployer();

        var result = await fixtures.Sut.Details(fixtures.DetailsRequest);
        var viewResult = result.VerifyReturnsViewModel();
        Assert.That(viewResult.ViewName, Is.EqualTo("Edit"));
    }

    [Test]
    public async Task GetDetails_Cohort_With_OtherParty_ShouldReturnViewPage()
    {
        var fixtures = new DetailsTestFixture()
            .WithCohortWithOtherParty();

        var result = await fixtures.Sut.Details(fixtures.DetailsRequest);
        var viewResult = result.VerifyReturnsViewModel();
        Assert.That(viewResult.ViewName, Is.EqualTo("View"));
    }

    [Test]
    public async Task PostDetails_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToCohortPage()
    {
        var fixtures = new DetailsTestFixture()
            .WithCohortWithEmployer()
            .WithCohort();

        var result = await fixtures.Sut.EditDraftApprenticeship(string.Empty, string.Empty, new EditDraftApprenticeshipViewModel { AccountHashedId = DetailsTestFixture.AccountHashedId, CohortId = DetailsTestFixture.CohortId, CohortReference = DetailsTestFixture.CohortReference, DraftApprenticeshipId = DetailsTestFixture.DraftApprenticeshipId });

        fixtures.OuterApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(DetailsTestFixture.CohortId, DetailsTestFixture.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipApimRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        result.VerifyReturnsRedirect();
    }

    [Test]
    public async Task PostDetails_WithValidModel_ShouldSaveDraftApprenticeshipAndRedirectToSelectOptionPage()
    {
        var fixtures = new DetailsTestFixture()
            .WithCohort();

        var result = await fixtures.Sut.EditDraftApprenticeship(string.Empty, string.Empty, new EditDraftApprenticeshipViewModel { AccountHashedId = DetailsTestFixture.AccountHashedId, CohortId = DetailsTestFixture.CohortId, CohortReference = DetailsTestFixture.CohortReference, DraftApprenticeshipId = DetailsTestFixture.DraftApprenticeshipId });

        fixtures.OuterApiClientMock.Verify(cs => cs.UpdateDraftApprenticeship(DetailsTestFixture.CohortId, DetailsTestFixture.DraftApprenticeshipId, It.IsAny<UpdateDraftApprenticeshipApimRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        var redirect = result.VerifyReturnsRedirectToActionResult();
        Assert.That(redirect.ActionName, Is.EqualTo("SelectOption"));
    }

    [Test]
    public async Task GetViewDetails_Cohort_With_Employer_ShouldReturnViewPage()
    {
        var fixtures = new DetailsTestFixture()
            .WithEmployerView();

        var result = await fixtures.Sut.ViewDetails(fixtures.DetailsRequest);
        var viewResult = result.VerifyReturnsViewModel();
        Assert.That(viewResult.ViewName, Is.EqualTo("View"));
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
        ApiErrors = [new("Field1", "Message1")];

        ModelMapperMock = new Mock<IModelMapper>();
        ModelMapperMock.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.Is<DetailsRequest>(dr => dr.DraftApprenticeshipId == DraftApprenticeshipId)))
            .ReturnsAsync(ViewModel.Object);

        ModelMapperMock.Setup(x => x.Map<ViewDraftApprenticeshipViewModel>(It.Is<DetailsRequest>(dr => dr.DraftApprenticeshipId == ViewDraftApprenticeshipId)))
            .ReturnsAsync(viewDraftApprenticeshipViewModel);


        Sut = new DraftApprenticeshipController(
            ModelMapperMock.Object,
            CommitmentsApiClientMock.Object,
            Mock.Of<IEncodingService>(),
            OuterApiClientMock.Object,
            Mock.Of<ICacheStorageService>());
    }

    private Mock<IModelMapper> ModelMapperMock { get; }
    private Mock<ICommitmentsApiClient> CommitmentsApiClientMock { get; }
    public Mock<IApprovalsApiClient> OuterApiClientMock { get; }
    private GetCohortResponse CohortDetails { get; set; }
    public static string AccountHashedId => "ACHID";
    public static long CohortId => 1;
    public static string CohortReference => "CHREF";
    public static long DraftApprenticeshipId => 99;
    private static long ViewDraftApprenticeshipId => 77;
    private static string DraftApprenticeshipHashedId => "DAHID";
    public GetDraftApprenticeshipResponse EditDraftApprenticeshipDetails { get; private set; }
    public DraftApprenticeshipController Sut { get; private set; }
    public List<ErrorDetail> ApiErrors { get; private set; }
    public readonly DetailsRequest DetailsRequest;
    public readonly Mock<IDraftApprenticeshipViewModel> ViewModel;

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
        DetailsRequest.DraftApprenticeshipId = ViewDraftApprenticeshipId;
        return this;
    }
}