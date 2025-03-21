﻿using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

public class AddDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
{
    public long AccountId { get; set; }
    public string AccountHashedId { get; set; }
    public string LegalEntityName { get; set; }
    public string TransferSenderHashedId { get; set; }
    public Guid? CacheKey { get; set; }
}