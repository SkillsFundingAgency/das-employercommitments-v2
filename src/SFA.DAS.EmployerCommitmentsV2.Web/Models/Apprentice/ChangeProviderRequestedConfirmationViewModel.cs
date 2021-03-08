namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ChangeProviderRequestedConfirmationViewModel 
    {
        public string AccountHashedId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public string ProviderName { get; set; }
        public string ApprenticeName { get; set; }
        public bool ProviderAddDetails { get; set; }
        public bool StoppedDuringCoP { get; set; }
    }
}
