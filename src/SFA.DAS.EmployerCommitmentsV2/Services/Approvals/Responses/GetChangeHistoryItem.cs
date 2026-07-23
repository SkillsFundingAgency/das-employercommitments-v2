using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetChangeHistoryItem
{
    public byte ChangeType { get; set; }
    public string Description { get; set; }
    public long ApprenticeshipId { get; set; }
    public string LearnerName { get; set; }
    public DateTime AppliedDate { get; set; }
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string ProviderName { get; set; }
}