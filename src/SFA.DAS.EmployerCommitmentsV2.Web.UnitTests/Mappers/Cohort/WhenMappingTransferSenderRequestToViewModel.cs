using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingTransferSenderRequestToViewModel
{
    private WhenMappingTransferSenderRequestToViewModelFixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new WhenMappingTransferSenderRequestToViewModelFixture();
    }

    [Test]
    public void OnlyTheCohortsWithTransferSenderAreMapped()
    {
        _fixture.Map();
        _fixture.Verify_Only_TheCohorts_WithTransferSender_Are_Mapped();
    }

    [Test]
    public void Then_TheCohortReferenceIsMapped()
    {
        _fixture.Map();
        _fixture.Verify_CohortReference_Is_Mapped();
    }

    [Test]
    public void Then_ProviderNameIsMapped()
    {
        _fixture.Map();
        _fixture.Verify_ProviderName_Is_Mapped();
    }

    [Test]
    public void Then_TransferSenderNameIsMapped()
    {
        _fixture.Map();
        _fixture.Verify_TransferSenderName_Is_Mapped();
    }

    [Test]
    public void Then_TransferSenderIdIsMapped()
    {
        _fixture.Map();
        _fixture.Verify_TransferSenderId_Is_Mapped();
    }

    [Test]
    public void Then_NumberOfApprenticesAreMapped()
    {
        _fixture.Map();
        _fixture.Verify_NumberOfApprentices_Are_Mapped();
    }

    [Test]
    public void Then_OrderBy_OnDateTransferred_Correctly()
    {
        _fixture.Map();
        _fixture.Verify_Ordered_By_OnDateTransferred();
    }

    [Test]
    public void Then_OrderBy_OnDateCreated_Correctly()
    {
        _fixture.MakeTheMessagesNull().SetCreatedOn();
        _fixture.Map();
        _fixture.Verify_Ordered_By_OnDateCreated();
    }

    [Test]
    public void Then_OrderBy_LatestMessageByEmployer_Correctly()
    {
        _fixture.MakeTheMessagesNull().SetLatestMessageFromEmployer();
        _fixture.Map();
        _fixture.Verify_Ordered_By_LatestMessageByEmployer();
    }

    [Test]
    public void Then_OrderBy_LatestMessageByProvider_Correctly()
    {
        _fixture.MakeTheMessagesNull().SetLatestMessageFromProvider();
        _fixture.Map();
        _fixture.Verify_Ordered_By_LatestMessageByProvider();
    }

    [Test]
    public void Then_AccountHashedId_IsMapped()
    {
        _fixture.Map();
        _fixture.Verify_AccountHashedId_IsMapped();
    }

    [Test]
    public void When_More_Than_One_TransferSender_Title_IsMapped()
    {
        _fixture.Map();
        _fixture.Verify_When_More_Than_One_TransferSender_Title_Is_Mapped();
    }

    [Test]
    public void When_Only_One_TransferSender_Title_IsMapped()
    {
        _fixture.SetOnlyOneTransferSender();
        _fixture.Map();
        _fixture.Verify_When_One_TransferSender_Title_Is_Mapped();
    }
}

public class WhenMappingTransferSenderRequestToViewModelFixture
{
    private readonly Mock<IEncodingService> _encodingService;
    private readonly CohortsByAccountRequest _cohortsByAccountRequest;
    private readonly GetCohortsResponse _getCohortsResponse;
    private readonly WithTransferSenderRequestViewModelMapper _mapper;
    private WithTransferSenderViewModel _withTransferSenderViewModel;

    private static long AccountId => 1;

    private static string AccountHashedId => "1AccountHashedId";

    public WhenMappingTransferSenderRequestToViewModelFixture()
    {
        _encodingService = new Mock<IEncodingService>();
        var commitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _cohortsByAccountRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
        _getCohortsResponse = CreateGetCohortsResponse();

        commitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).ReturnsAsync(_getCohortsResponse);
        _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

        _mapper = new WithTransferSenderRequestViewModelMapper(commitmentsApiClient.Object, _encodingService.Object,Mock.Of<IUrlHelper>());
    }

    public WhenMappingTransferSenderRequestToViewModelFixture Map()
    {
        _withTransferSenderViewModel = _mapper.Map(_cohortsByAccountRequest).Result;
        return this;
    }

    public void Verify_Only_TheCohorts_WithTransferSender_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_withTransferSenderViewModel.Cohorts.Count(), Is.EqualTo(2));
            Assert.That(GetCohortInTransferSenderViewModel(1), Is.Not.Null);
            Assert.That(GetCohortInTransferSenderViewModel(2), Is.Not.Null);
        });
    }

    public void Verify_CohortReference_Is_Mapped()
    {
        _encodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInTransferSenderViewModel(1).CohortReference, Is.EqualTo("1_Encoded"));
            Assert.That(GetCohortInTransferSenderViewModel(2).CohortReference, Is.EqualTo("2_Encoded"));
        });
    }

    public void Verify_ProviderName_Is_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInTransferSenderViewModel(1).ProviderName, Is.EqualTo("Provider1"));
            Assert.That(GetCohortInTransferSenderViewModel(2).ProviderName, Is.EqualTo("Provider2"));
        });
    }

    public void Verify_TransferSenderName_Is_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInTransferSenderViewModel(1).TransferSenderName, Is.EqualTo("TransferSender1"));
            Assert.That(GetCohortInTransferSenderViewModel(2).TransferSenderName, Is.EqualTo("TransferSender2"));
        });
    }

    public void Verify_TransferSenderId_Is_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInTransferSenderViewModel(1).TransferSenderId, Is.EqualTo(1));
            Assert.That(GetCohortInTransferSenderViewModel(2).TransferSenderId, Is.EqualTo(2));
        });
    }

    public void Verify_NumberOfApprentices_Are_Mapped()
    {
        Assert.Multiple(() =>
        {
            Assert.That(GetCohortInTransferSenderViewModel(1).NumberOfApprentices, Is.EqualTo(100));
            Assert.That(GetCohortInTransferSenderViewModel(2).NumberOfApprentices, Is.EqualTo(200));
        });
    }

    public void Verify_Ordered_By_OnDateTransferred()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_withTransferSenderViewModel.Cohorts.First().CohortReference, Is.EqualTo("1_Encoded"));
            Assert.That(_withTransferSenderViewModel.Cohorts.Last().CohortReference, Is.EqualTo("2_Encoded"));
        });
    }

    public void Verify_Ordered_By_OnDateCreated()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_withTransferSenderViewModel.Cohorts.First().CohortReference, Is.EqualTo("2_Encoded"));
            Assert.That(_withTransferSenderViewModel.Cohorts.Last().CohortReference, Is.EqualTo("1_Encoded"));
        });
    }

    public void Verify_Ordered_By_LatestMessageByEmployer()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_withTransferSenderViewModel.Cohorts.First().CohortReference, Is.EqualTo("2_Encoded"));
            Assert.That(_withTransferSenderViewModel.Cohorts.Last().CohortReference, Is.EqualTo("1_Encoded"));
        });
    }

    public void Verify_Ordered_By_LatestMessageByProvider()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_withTransferSenderViewModel.Cohorts.First().CohortReference, Is.EqualTo("2_Encoded"));
            Assert.That(_withTransferSenderViewModel.Cohorts.Last().CohortReference, Is.EqualTo("1_Encoded"));
        });
    }

    public void Verify_AccountHashedId_IsMapped()
    {
        Assert.That(_withTransferSenderViewModel.AccountHashedId, Is.EqualTo(AccountHashedId));
    }

    public void Verify_When_More_Than_One_TransferSender_Title_Is_Mapped()
    {
        Assert.That(_withTransferSenderViewModel.Title, Is.EqualTo(WithTransferSenderRequestViewModelMapper.Title + "s"));
    }

    public void Verify_When_One_TransferSender_Title_Is_Mapped()
    {
        Assert.That(_withTransferSenderViewModel.Title, Is.EqualTo(WithTransferSenderRequestViewModelMapper.Title));
    }

    public void SetOnlyOneTransferSender()
    {
        foreach (var resp in _getCohortsResponse.Cohorts)
        {
            resp.TransferSenderId = 1;
            resp.TransferSenderName = "TransferSender1";
        }
    }

    public WhenMappingTransferSenderRequestToViewModelFixture MakeTheMessagesNull()
    {
        foreach (var cohortSummary in _getCohortsResponse.Cohorts)
        {
            cohortSummary.LatestMessageFromEmployer = cohortSummary.LatestMessageFromProvider = null;
        }

        return this;
    }

    public WhenMappingTransferSenderRequestToViewModelFixture SetCreatedOn()
    {
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 1).CreatedOn = DateTime.Now.AddMinutes(-5);
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 2).CreatedOn = DateTime.Now.AddMinutes(-7);
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 3).CreatedOn = DateTime.Now.AddMinutes(-9);
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 4).CreatedOn = DateTime.Now.AddMinutes(-10);
        return this;
    }

    public WhenMappingTransferSenderRequestToViewModelFixture SetLatestMessageFromEmployer()
    {
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 1).LatestMessageFromEmployer = 
            new Message("1st Message", DateTime.Now.AddMinutes(-6));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 2).LatestMessageFromEmployer =
            new Message("2nd Message", DateTime.Now.AddMinutes(-7));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 3).LatestMessageFromEmployer = 
            new Message("3rd Message", DateTime.Now.AddMinutes(-8));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 4).LatestMessageFromEmployer = 
            new Message("4th Message", DateTime.Now.AddMinutes(-9));
            
        return this;
    }

    public WhenMappingTransferSenderRequestToViewModelFixture SetLatestMessageFromProvider()
    {
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 1).LatestMessageFromProvider =
            new Message("1st Message", DateTime.Now.AddMinutes(-6));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 2).LatestMessageFromProvider =
            new Message("2nd Message", DateTime.Now.AddMinutes(-7));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 3).LatestMessageFromProvider =
            new Message("3rd Message", DateTime.Now.AddMinutes(-8));
        _getCohortsResponse.Cohorts.First(x => x.CohortId == 4).LatestMessageFromProvider =
            new Message("4th Message", DateTime.Now.AddMinutes(-9));

        return this;
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
                TransferSenderId = 1,
                TransferSenderName = "TransferSender1",
                ProviderName = "Provider1",
                NumberOfDraftApprentices = 100,
                IsDraft = false,
                WithParty = Party.TransferSender,
                LatestMessageFromEmployer = new Message("this is the last message from Employer", DateTime.Now.AddMinutes(-10)),
                LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-11))
            },
            new()
            {
                CohortId = 2,
                AccountId = 1,
                ProviderId = 2,
                TransferSenderId = 2,
                TransferSenderName = "TransferSender2",
                ProviderName = "Provider2",
                NumberOfDraftApprentices = 200,
                IsDraft = false,
                WithParty = Party.TransferSender,
                CreatedOn = DateTime.Now.AddMinutes(-8),
                LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-8)),
                LatestMessageFromEmployer = new Message("This is latestMessage from Employer", DateTime.Now.AddMinutes(-7))
            },
            new()
            {
                CohortId = 3,
                AccountId = 1,
                ProviderId = 2,
                TransferSenderId = 2,
                TransferSenderName = "TransferSender2",
                ProviderName = "Provider3",
                NumberOfDraftApprentices = 300,
                IsDraft = false,
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
                IsDraft = true,
                WithParty = Party.Employer,
                CreatedOn = DateTime.Now
            },
        };

        return new GetCohortsResponse(cohorts);
    }

    private static long GetCohortId(string cohortReference)
    {
        return long.Parse(cohortReference.Replace("_Encoded", ""));
    }

    private WithTransferSenderCohortSummaryViewModel GetCohortInTransferSenderViewModel(long id)
    {
        return _withTransferSenderViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
    }
}