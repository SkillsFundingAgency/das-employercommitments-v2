namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectAcceptedLevyTransferConnectionViewModel
{
    public string AccountHashedId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public string ApplicationAndSenderHashedId { get; set; }
    public List<LevyTransferDisplayConnection> Applications { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }
}


public class LevyTransferDisplayConnection
{
    public long Id { get; set; }
    public string ApplicationHashedId { get; set; }
    public string OpportunityHashedId { get; set; }
    public string SendingEmployerPublicHashedId { get; set; }
    public string DisplayName { get; set; }
    public string ApplicationAndSenderHashedId { get; set; }
}