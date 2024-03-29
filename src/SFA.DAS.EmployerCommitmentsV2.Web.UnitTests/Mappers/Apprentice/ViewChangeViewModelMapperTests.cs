﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ViewChangeViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
    private Mock<IEncodingService> _mockEncodingService;

    private GetChangeOfPartyRequestsResponse _changeOfPartyResponse;
    private GetApprenticeshipResponse _apprenticeshipResponse;
    private GetPriceEpisodesResponse _priceEpisodesResponse;
    private GetProviderResponse _providerResponse;

    private const long ProviderId = 10000;
    private const long CohortId = 10001;
    private const string CohortReference = "ABC123";

    private readonly DateTime _oldFromDate = DateTime.Now.AddDays(-30);
    private readonly DateTime _oldToDate = DateTime.Now.AddDays(-15);
    private readonly DateTime _newFromDate = DateTime.Now.AddDays(-15);
    private readonly DateTime? _newToDate = null;

    private const int NewPrice = 1500;

    private Fixture _autoFixture;
    private ViewChangesRequest _request;
    private ViewChangesViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _autoFixture = new Fixture();

        _changeOfPartyResponse = _autoFixture.Build<GetChangeOfPartyRequestsResponse>()
            .With(r => r.ChangeOfPartyRequests, GetChangeOfPartyRequestsMock())
            .Create();
        _apprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>()
            .Create();
        _priceEpisodesResponse = _autoFixture.Build<GetPriceEpisodesResponse>()
            .With(p => p.PriceEpisodes, GetPriceEpisodesMock())
            .Create();
        _providerResponse = _autoFixture.Build<GetProviderResponse>()
            .Create();
        _request = _autoFixture.Build<ViewChangesRequest>()
            .Create();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_changeOfPartyResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_priceEpisodesResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_providerResponse);

        _mockEncodingService = new Mock<IEncodingService>();
        _mockEncodingService.Setup(c => c.Encode(It.IsAny<long>(), EncodingType.CohortReference))
            .Returns(CohortReference);

        _mockCommitmentsApiClient.Setup(c => c.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetCohortResponseMock());

        _mapper = new ViewChangesViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object);
    }

    [Test]
    public async Task ChangeOfPartyRequestIsCalled()
    {
        var result = await _mapper.Map(_request);
            
        _mockCommitmentsApiClient.Verify(c => c.GetChangeOfPartyRequests(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetPriceHistoryIsCalled()
    {
        var result = await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetApprenticeshipIsCalled()
    {
        var result = await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task WhenMoreThanOnePriceIsFoundInHistory_ThenTheMostRecentIsUsed()
    {
        _priceEpisodesResponse = _autoFixture.Build<GetPriceEpisodesResponse>()
            .With(p => p.PriceEpisodes, GetPriceEpisodesWithMultipleChangesMock())
            .Create();
        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_priceEpisodesResponse);

        var result = await _mapper.Map(_request);

        Assert.That(result.CurrentPrice, Is.EqualTo(NewPrice));
    }

    [Test]
    public async Task WhenMoreThanOneChangeOfProviderRequestIsFound_ThenThePendingRequestIsMapped()
    {
        _changeOfPartyResponse = _autoFixture.Build<GetChangeOfPartyRequestsResponse>()
            .With(p => p.ChangeOfPartyRequests, GetChangeOfPartyRequestsWithMultipleRequestsMock())
            .Create();
        _mockCommitmentsApiClient.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_changeOfPartyResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetProvider(ProviderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetProviderResponse { Name = "NewProvider" });

        var result = await _mapper.Map(_request);

        Assert.That(result.NewProviderName, Is.EqualTo("NewProvider"));
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(_request.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.AccountHashedId, Is.EqualTo(_request.AccountHashedId));
    }

    [Test]
    public async Task ApprenticeName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.ApprenticeName, Is.EqualTo($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}"));
    }

    [Test]
    public async Task CurrentProviderName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.CurrentProviderName, Is.EqualTo(_apprenticeshipResponse.ProviderName));
    }

    [Test]
    public async Task CurrentStartDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.CurrentStartDate, Is.EqualTo(_apprenticeshipResponse.StartDate));
    }

    [Test]
    public async Task CurrentEndDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.CurrentEndDate, Is.EqualTo(_apprenticeshipResponse.EndDate));
    }

    [Test]
    public async Task NewProviderName_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewProviderName, Is.EqualTo(_providerResponse.Name));
    }

    [Test]
    public async Task NewStartDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewStartDate, Is.EqualTo(_newFromDate));
    }

    [Test]
    public async Task NewEndDate_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewEndDate, Is.EqualTo(_newToDate));
    }

    [Test]
    public async Task NewPrice_IsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.NewPrice, Is.EqualTo(NewPrice));
    }

    private List<ChangeOfPartyRequest> GetChangeOfPartyRequestsMock()
    {
        return
        [
            new ChangeOfPartyRequest
            {
                ChangeOfPartyType = ChangeOfPartyRequestType.ChangeProvider,
                Status = ChangeOfPartyRequestStatus.Pending,
                ProviderId = ProviderId,
                CohortId = CohortId,
                Price = NewPrice,
                StartDate = _newFromDate,
                WithParty = Party.Provider
            }
        ];
    }

    private static GetCohortResponse GetCohortResponseMock()
    {
        var response = new GetCohortResponse
        {
            CohortId = CohortId
        };

        return response;
    }

    private List<PriceEpisode> GetPriceEpisodesMock()
    {
        return
        [
            new PriceEpisode
            {
                ApprenticeshipId = _apprenticeshipResponse.Id,
                Cost = 1000,
                FromDate = DateTime.Now.AddDays(-30)
            }
        ];
    }

    private List<PriceEpisode> GetPriceEpisodesWithMultipleChangesMock()
    {
        return
        [
            new PriceEpisode
            {
                ApprenticeshipId = _apprenticeshipResponse.Id,
                Cost = 1000,
                FromDate = _oldFromDate,
                ToDate = _oldToDate
            },

            new PriceEpisode
            {
                ApprenticeshipId = _apprenticeshipResponse.Id,
                Cost = NewPrice,
                FromDate = _newFromDate,
                ToDate = null
            }
        ];
    }

    private List<ChangeOfPartyRequest> GetChangeOfPartyRequestsWithMultipleRequestsMock()
    {
        return
        [
            new ChangeOfPartyRequest
            {
                ChangeOfPartyType = ChangeOfPartyRequestType.ChangeProvider,
                StartDate = _oldFromDate,
                EndDate = _oldToDate,
                ProviderId = 99999,
                Price = 1000,
                Status = ChangeOfPartyRequestStatus.Approved,
                WithParty = Party.Provider
            },


            new ChangeOfPartyRequest
            {
                ChangeOfPartyType = ChangeOfPartyRequestType.ChangeProvider,
                StartDate = _oldFromDate,
                EndDate = _newToDate,
                ProviderId = ProviderId,
                CohortId = CohortId,
                Price = NewPrice,
                Status = ChangeOfPartyRequestStatus.Pending,
                WithParty = Party.Provider
            }
        ];
    }
}