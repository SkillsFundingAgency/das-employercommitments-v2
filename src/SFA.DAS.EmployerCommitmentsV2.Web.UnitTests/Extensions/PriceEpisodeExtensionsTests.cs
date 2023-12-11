using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;

[TestFixture]
public class PriceEpisodeExtensionsTests
{
    private List<GetPriceEpisodesResponse.PriceEpisode> _priceEpisodes;

    [SetUp]
    public void Arrange()
    {
        _priceEpisodes =
        [
            CreatePriceEpisode(1, new DateTime(2018, 01, 01), new DateTime(2018, 12, 31)),
            CreatePriceEpisode(2, new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)),
            CreatePriceEpisode(3, new DateTime(2020, 01, 01), null)
        ];
    }

    [TestCase("2017-01-01", 1, Description = "First price episode if all future-dated")]
    [TestCase("2018-01-01", 1, Description = "First day of an episode")]
    [TestCase("2018-12-31", 1, Description = "Last day of an episode")]
    [TestCase("2019-06-01", 2, Description = "Episode mid-point")]
    [TestCase("2020-06-01", 3, Description = "Within an episode without an end date")]
    public void PriceIsDeterminedCorrectly(DateTime effectiveDate, decimal expectedCost)
    {
        Assert.That(_priceEpisodes.GetPrice(effectiveDate), Is.EqualTo(expectedCost));
    }

    private static GetPriceEpisodesResponse.PriceEpisode CreatePriceEpisode(decimal cost, DateTime from, DateTime? to)
    {
        return new GetPriceEpisodesResponse.PriceEpisode { Cost = cost, FromDate = from, ToDate = to };
    }
}