
namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ViewChangesViewModel
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }

        public string ApprenticeName { get; set; }
        public string OldProviderName { get; set; }
        
        public string NewProviderName { get; set; }
    }
}
