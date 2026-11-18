using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class GetAllChangeHistoryListViewModel
{
    public List<GetAllChangeHistoryViewModel> ChangeHistory { get; set; } = [];
    public string AccountHashedId { get; set; }
    public DateTime AvailableFrom { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public int TotalPages => PaginationHelper.GetTotalPages(TotalCount, PageSize);
    public bool ShowPageLinks => TotalPages > 1;
    public int PagedRecordsFrom => TotalCount == 0 ? 0 : (PageNumber - 1) * PageSize + 1;
    public int PagedRecordsTo => Math.Min(PageNumber * PageSize, TotalCount);

    public IEnumerable<PageLink> PageLinks => PaginationHelper.BuildPageLinks(PageNumber, TotalPages, BuildPagedRouteData);

    private Dictionary<string, string> BuildPagedRouteData(int pageNumber) => new()
    {
        { nameof(AccountHashedId), AccountHashedId },
        { nameof(PageNumber), pageNumber.ToString() }
    };
}

public class GetAllChangeHistoryViewModel
{
    public DateTime AppliedDate { get; set; }
    public string Description { get; set; }
    public LearningChangeType ChangeType { get; set; }
    public Guid Id { get; set; }
    public string LearnerName { get; set; }
    public string ProviderName { get; set; }
}