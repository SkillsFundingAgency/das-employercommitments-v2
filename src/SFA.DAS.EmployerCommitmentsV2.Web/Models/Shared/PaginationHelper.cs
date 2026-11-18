namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

public static class PaginationHelper
{
    public static int GetTotalPages(int totalCount, int pageSize) => (int)Math.Ceiling((double)totalCount / pageSize);

    public static IEnumerable<PageLink> BuildPageLinks(int pageNumber, int totalPages, Func<int, Dictionary<string, string>> buildRouteData)
    {
        var links = new List<PageLink>();
        var totalPageLinks = totalPages < 5 ? totalPages : 5;

        if (totalPages > 1 && pageNumber > 1)
        {
            links.Add(new PageLink
            {
                Label = "Previous",
                AriaLabel = "Previous page",
                RouteData = buildRouteData(pageNumber - 1)
            });
        }

        var pageNumberSeed = 1;
        if (totalPages > 5 && pageNumber > 3)
        {
            pageNumberSeed = pageNumber - 2;

            if (pageNumber > totalPages - 2)
                pageNumberSeed = totalPages - 4;
        }

        for (var i = 0; i < totalPageLinks; i++)
        {
            links.Add(new PageLink
            {
                Label = (pageNumberSeed + i).ToString(),
                AriaLabel = $"Page {pageNumberSeed + i}",
                IsCurrent = pageNumberSeed + i == pageNumber ? true : (bool?)null,
                RouteData = buildRouteData(pageNumberSeed + i)
            });
        }

        if (totalPages > 1 && pageNumber < totalPages)
        {
            links.Add(new PageLink
            {
                Label = "Next",
                AriaLabel = "Next page",
                RouteData = buildRouteData(pageNumber + 1)
            });
        }

        return links;
    }
}
