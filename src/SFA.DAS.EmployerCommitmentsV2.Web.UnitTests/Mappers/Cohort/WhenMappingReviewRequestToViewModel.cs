using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingReviewRequestToViewModel
{
    [Test]
    public void OnlyTheCohortsReadyForReviewAreMapped()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_OnlyTheCohorts_ReadyForReviewForEmployer_Are_Mapped();
    }

    [Test]
    public void Then_TheCohortReferenceIsMapped()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_CohortReference_Is_Mapped();
    }

    [Test]
    public void Then_ProviderNameIsMapped()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_ProviderName_Is_Mapped();
    }

    [Test]
    public void Then_NumberOfApprenticesAreMapped()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_NumberOfApprentices_Are_Mapped();
    }

    [Test]
    public void Then_LastMessage_IsMapped_Correctly()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_LastMessage_Is_MappedCorrectly();
    }

    [Test]
    public void Then_Cohort_OrderBy_OnDateCreated_Correctly()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_Ordered_By_DateCreated();
    }

    [Test]
    public void Then_AccountHashedId_IsMapped()
    {
        var fixture = new WhenMappingReviewRequestToViewModelFixture();
        fixture.Map();

        fixture.Verify_AccountHashedId_IsMapped();
    }
}

public class WhenMappingReviewRequestToViewModelFixture
{
    private readonly Mock<IEncodingService> _encodingService;
    private readonly CohortsByAccountRequest _reviewRequest;
    private readonly ReviewRequestViewModelMapper _mapper;
    private ReviewViewModel _reviewViewModel;
    
    private static long AccountId => 1;

    private static string AccountHashedId => "1AccountHashedId";

    public WhenMappingReviewRequestToViewModelFixture()
    {
        _encodingService = new Mock<IEncodingService>();
        var commitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _reviewRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
        var getCohortsResponse = CreateGetCohortsResponse();
           
        commitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(getCohortsResponse));
        _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

        _mapper = new ReviewRequestViewModelMapper(commitmentsApiClient.Object, _encodingService.Object, Mock.Of<IUrlHelper>());
    }

    public WhenMappingReviewRequestToViewModelFixture Map()
    {
        _reviewViewModel = _mapper.Map(_reviewRequest).Result;
        return this;
    }

    public void Verify_OnlyTheCohorts_ReadyForReviewForEmployer_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_reviewViewModel.Cohorts.Count(), Is.EqualTo(2));

            Assert.That(GetCohortInReviewViewModel(1), Is.Not.Null);
            Assert.That(GetCohortInReviewViewModel(2), Is.Not.Null);
        });
    }

    public void Verify_CohortReference_Is_Mapped()
    {
        _encodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInReviewViewModel(1).CohortReference, Is.EqualTo("1_Encoded"));
            Assert.That(GetCohortInReviewViewModel(2).CohortReference, Is.EqualTo("2_Encoded"));
        });
    }

    public void Verify_ProviderName_Is_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInReviewViewModel(1).ProviderName, Is.EqualTo("Provider1"));
            Assert.That(GetCohortInReviewViewModel(2).ProviderName, Is.EqualTo("Provider2"));
        });
    }

    public void Verify_NumberOfApprentices_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInReviewViewModel(1).NumberOfApprentices, Is.EqualTo(100));
            Assert.That(GetCohortInReviewViewModel(2).NumberOfApprentices, Is.EqualTo(200));
        });
    }

    public void Verify_LastMessage_Is_MappedCorrectly()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInReviewViewModel(1).LastMessage, Is.EqualTo("No message added"));
            Assert.That(GetCohortInReviewViewModel(2).LastMessage, Is.EqualTo("This is latestMessage from provider"));
        });
    }

    public void Verify_Ordered_By_DateCreated()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_reviewViewModel.Cohorts.First().CohortReference, Is.EqualTo("2_Encoded"));
            Assert.That(_reviewViewModel.Cohorts.Last().CohortReference, Is.EqualTo("1_Encoded"));
        });
    }

    public void Verify_AccountHashedId_IsMapped()
    {
        Assert.That(_reviewViewModel.AccountHashedId, Is.EqualTo(AccountHashedId));
    }

    private static GetCohortsResponse CreateGetCohortsResponse()
    {
        IEnumerable<CohortSummary> cohorts = new List<CohortSummary>()
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
                IsDraft = false,
                WithParty = Party.Employer,
                CreatedOn = DateTime.Now.AddMinutes(-5),
                LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-2))
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
        return long.Parse(cohortReference.Replace("_Encoded",""));
    }

    private ReviewCohortSummaryViewModel GetCohortInReviewViewModel(long id)
    {
        return _reviewViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
    }
}