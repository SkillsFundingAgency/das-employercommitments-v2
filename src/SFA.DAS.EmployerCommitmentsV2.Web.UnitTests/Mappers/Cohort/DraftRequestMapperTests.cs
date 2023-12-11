using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingDraftRequestToViewModel
{
    [Test]
    public void OnlyTheCohortsReadyForDraftAreMapped()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_OnlyTheDraftCohorts_WithEmployer_Are_Mapped();
    }

    [Test]
    public void Then_TheCohortReferenceIsMapped()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_CohortRefrence_Is_Mapped();
    }

    [Test]
    public void Then_ProviderNameIsMapped()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_ProviderName_Is_Mapped();
    }

    [Test]
    public void Then_NumberOfApprenticesAreMapped()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_NumberOfApprentices_Are_Mapped();
    }

    [Test]
    public void Then_Cohort_OrderBy_OnDateCreated_Correctly()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_Ordered_By_DateCreated();
    }

    [Test]
    public void Then_AccountHashedId_IsMapped()
    {
        var fixture = new WhenMappingDraftRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_AccountHashedId_IsMapped();
    }
}

public class WhenMappingDraftRequestToViewModelFixture
{
    private Mock<IEncodingService> EncodingService { get; set; }
    private Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
    private Mock<IUrlHelper> UrlHelper { get; }
    private CohortsByAccountRequest DraftRequest { get; set; }
    private GetCohortsResponse GetCohortsResponse { get; set; }
    private DraftRequestMapper Mapper { get; set; }
    private DraftViewModel DraftViewModel { get; set; }

    public static long AccountId => 1;

    public static string AccountHashedId => "1AccountHashedId";

    public WhenMappingDraftRequestToViewModelFixture()
    {
        EncodingService = new Mock<IEncodingService>();
        CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        DraftRequest = new CohortsByAccountRequest { AccountId = AccountId, AccountHashedId = AccountHashedId };
        GetCohortsResponse = CreateGetCohortsResponse();

        CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(GetCohortsResponse));
        EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

        UrlHelper = new Mock<IUrlHelper>();
        UrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns<UrlActionContext>((ac) => $"http://{ac.Controller}/{ac.Action}/");

        Mapper = new DraftRequestMapper(CommitmentsApiClient.Object, EncodingService.Object, UrlHelper.Object);
    }

    public WhenMappingDraftRequestToViewModelFixture Map()
    {
        DraftViewModel = Mapper.Map(DraftRequest).Result;
        return this;
    }

    public void Verify_OnlyTheDraftCohorts_WithEmployer_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DraftViewModel.Cohorts.Count(), Is.EqualTo(2));

            Assert.That(GetDraftCohortWithId(2), Is.Not.Null);
            Assert.That(GetDraftCohortWithId(3), Is.Not.Null);
        });
    }

    public void Verify_CohortRefrence_Is_Mapped()
    {
        EncodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

        Assert.Multiple(() =>
        {
            Assert.That(GetDraftCohortWithId(2).CohortReference, Is.EqualTo("2_Encoded"));
            Assert.That(GetDraftCohortWithId(3).CohortReference, Is.EqualTo("3_Encoded"));
        });
    }

    public void Verify_ProviderName_Is_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetDraftCohortWithId(2).ProviderName, Is.EqualTo("Provider2"));
            Assert.That(GetDraftCohortWithId(3).ProviderName, Is.EqualTo("Provider3"));
        });
    }

    public void Verify_NumberOfApprentices_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetDraftCohortWithId(2).NumberOfApprentices, Is.EqualTo(200));
            Assert.That(GetDraftCohortWithId(3).NumberOfApprentices, Is.EqualTo(300));
        });
    }

    public void Verify_Ordered_By_DateCreated()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DraftViewModel.Cohorts.First().CohortReference, Is.EqualTo("3_Encoded"));
            Assert.That(DraftViewModel.Cohorts.Last().CohortReference, Is.EqualTo("2_Encoded"));
        });
    }

    public void Verify_AccountHashedId_IsMapped()
    {
        Assert.That(DraftViewModel.AccountHashedId, Is.EqualTo(AccountHashedId));
    }

    private GetCohortsResponse CreateGetCohortsResponse()
    {
        IEnumerable<CohortSummary> cohorts = new List<CohortSummary>
        {
            new()
            {
                CohortId = 1,
                AccountId = 1,
                ProviderId = 1,
                ProviderName = "Provider1",
                NumberOfDraftApprentices = 100,
                IsDraft = false,
                WithParty = Party.Employer,
                CreatedOn = DateTime.Now.AddMinutes(-3)
            },
            new()
            {
                CohortId = 2,
                AccountId = 1,
                ProviderId = 2,
                ProviderName = "Provider2",
                NumberOfDraftApprentices = 200,
                IsDraft = true,
                WithParty = Party.Employer,
                CreatedOn = DateTime.Now.AddMinutes(-5),
            },
            new()
            {
                CohortId = 3,
                AccountId = 1,
                ProviderId = 3,
                ProviderName = "Provider3",
                NumberOfDraftApprentices = 300,
                IsDraft = true,
                WithParty = Party.Employer,
                CreatedOn = DateTime.Now.AddMinutes(-1)
            },
            new()
            {
                CohortId = 4,
                AccountId = 1,
                ProviderId = 4,
                ProviderName = "Provider4",
                NumberOfDraftApprentices = 400,
                IsDraft = false,
                WithParty = Party.Provider,
                CreatedOn = DateTime.Now
            },
        };

        return new GetCohortsResponse(cohorts);
    }

    private static long GetCohortId(string cohortReference)
    {
        return long.Parse(cohortReference.Replace("_Encoded", ""));
    }

    private DraftCohortSummaryViewModel GetDraftCohortWithId(long id)
    {
        return DraftViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
    }
}