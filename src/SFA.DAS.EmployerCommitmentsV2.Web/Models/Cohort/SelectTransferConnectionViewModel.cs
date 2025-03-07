﻿using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectTransferConnectionViewModel
{
    public string AccountHashedId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public bool IsLevyAccount { get; set; }

    public string TransferConnectionCode { get; set; }

    public List<TransferConnection> TransferConnections { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }
}
