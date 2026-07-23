namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class GetAllChangeHistoryListViewModel
{
    public List<GetAllChangeHistoryViewModel> ChangeHistory { get; set; } = [];
    public string AccountHashedId { get; set; }
    public DateTime AvailableFrom { get; set; }
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