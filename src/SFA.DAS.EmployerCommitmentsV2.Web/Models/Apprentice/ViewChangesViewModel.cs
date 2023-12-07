using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ViewChangesViewModel
{
    public string AccountHashedId { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public string ApprenticeName { get; set; }
    public string CurrentProviderName { get; set; }
    public DateTime CurrentStartDate { get; set; }
    public DateTime CurrentEndDate { get; set; }
    public int CurrentPrice { get; set; }
    public string NewProviderName { get; set; }
    public DateTime? NewStartDate { get; set; }
    public DateTime? NewEndDate { get; set; }
    public int? NewPrice { get; set; }
    public Party CurrentParty { get; set; }
    public string CohortReference { get; set; }
}