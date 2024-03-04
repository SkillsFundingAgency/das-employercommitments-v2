using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.CommitmentsV2.Types;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ConfirmDetailsAndSendViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private ChangeOfProviderRequest _request;

    private GetApprenticeshipResponse _apprenticeshipResponse;
    private GetPriceEpisodesResponse _priceEpisodeResponse;
        
    private ConfirmDetailsAndSendViewModelMapper _mapper;
    private TrainingProgramme _standardSummary;

    [SetUp]
    public void Arrange()
    {
        var autoFixture = new Fixture();

        _request = autoFixture.Build<ChangeOfProviderRequest>()
            .With(x => x.NewStartMonth,1)
            .With(x => x.NewStartYear, 2020)
            .With(x => x.NewEndMonth,2)
            .With(x => x.NewEndYear,2022)
            .With(x => x.NewPrice, 1500)
            .Create();

        _apprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
            .With(x => x.CourseCode, "ABC")
            .With(x => x.StartDate, new DateTime(2020, 1, 1))
            .With(x => x.StartDate, new DateTime(2021, 1, 1))
            .Create();

        _priceEpisodeResponse = autoFixture.Build<GetPriceEpisodesResponse>()
            .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                new PriceEpisode { Cost = 1000, ToDate = DateTime.Now.AddMonths(-1)},
                new PriceEpisode { Cost = 2000, ToDate = null } })
            .Create();

        _standardSummary = autoFixture.Create<TrainingProgramme>();
        _standardSummary.EffectiveFrom = new DateTime(2018, 1, 1);
        _standardSummary.EffectiveTo = new DateTime(2022, 1, 1);
        _standardSummary.FundingPeriods = SetPriceBand(1000);

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_apprenticeshipResponse);
        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_priceEpisodeResponse);
        _mockCommitmentsApiClient.Setup(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = _standardSummary
            });

        _mapper = new ConfirmDetailsAndSendViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test]
    public async Task GetFundingCapIsCalled()
    {
        await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(t => t.GetTrainingProgramme(_apprenticeshipResponse.CourseCode, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task GetPriceEpisodesCapIsCalled()
    {
        await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Current_PriceEpisodeIsMapped()
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.CurrentPrice, Is.EqualTo(2000));
    }

    [Test]
    public async Task GetApprenticeshipCapIsCalled()
    {
        await _mapper.Map(_request);

        _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task FundingBandCap_IsMapped() 
    {
        var result = await _mapper.Map(_request);

        Assert.That(result.MaxFunding, Is.EqualTo(1000));
    }

    [TestCase(2000, true)]
    [TestCase(500, false)]
    public async Task ExceedsMaxFunding_IsMapped(int newPrice, bool expectsExceedsMaxFunding)
    {
        _request.NewPrice = newPrice;

        var result = await _mapper.Map(_request);

        Assert.That(result.ExceedsMaxFunding, Is.EqualTo(expectsExceedsMaxFunding));
    }

    private static List<TrainingProgrammeFundingPeriod> SetPriceBand(int fundingCap)
    {
        return
        [
            new TrainingProgrammeFundingPeriod
            {
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = DateTime.Now.AddMonths(1),
                FundingCap = fundingCap
            }
        ];
    }
}