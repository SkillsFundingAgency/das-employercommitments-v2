using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse.GetPriceEpisodeResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class PriceEpisodeExtenstions
{
    public static int GetPrice(this IEnumerable<GetPriceEpisodesResponse.PriceEpisode> priceEpisodes)
    {
        return priceEpisodes.GetPrice(DateTime.UtcNow);
    }

    public static int GetPrice(this IEnumerable<GetPriceEpisodesResponse.PriceEpisode> priceEpisodes,
        DateTime effectiveDate)
    {
        var episodes = priceEpisodes.ToList();

        var episode = episodes.SingleOrDefault(x =>
            x.FromDate <= effectiveDate && (x.ToDate == null || x.ToDate >= effectiveDate));

        return (int)(episode?.Cost ?? episodes.First().Cost);
    }

    public static PriceEpisode GetPriceEpisode(this IEnumerable<PriceEpisode> priceEpisodes)
    {
        return priceEpisodes.GetPriceEpisode(DateTime.UtcNow);
    }

    private static PriceEpisode GetPriceEpisode(this IEnumerable<PriceEpisode> priceEpisodes, DateTime effectiveDate)
    {
        var episodes = priceEpisodes.ToList();

        var episode = episodes.FirstOrDefault(x =>
            x.FromDate <= effectiveDate && (x.ToDate == null || x.ToDate >= effectiveDate));

        return episode;
    }
}