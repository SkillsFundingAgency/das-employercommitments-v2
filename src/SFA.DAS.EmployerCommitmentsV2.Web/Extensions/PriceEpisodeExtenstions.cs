using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.GetManageApprenticeshipDetailsResponse.GetPriceEpisodeResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
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

        public static int GetPrice(this IEnumerable<PriceEpisode> priceEpisodes)
        {
            return priceEpisodes.GetPrice(DateTime.UtcNow);
        }

        public static int GetPrice(this IEnumerable<PriceEpisode> priceEpisodes,
            DateTime effectiveDate)
        {
            var episodes = priceEpisodes.ToList();

            var episode = episodes.FirstOrDefault(x =>
                x.FromDate <= effectiveDate && (x.ToDate == null || x.ToDate >= effectiveDate));

            return (int)(episode?.Cost ?? episodes.FirstOrDefault().Cost);
        }

    }
}
