﻿using Newtonsoft.Json;
using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class ViewApprenticeshipUpdatesViewModel : 
    IApprenticeshipUpdatesViewModel, IAuthorizationContextModel
{        
    public bool? UndoChanges { get; set; }
    [FromRoute]
    public string AccountHashedId { get; set; }
    [JsonIgnore]
    public long AccountId { get; set; }
    [FromRoute]
    public string ApprenticeshipHashedId { get; set; }
    [JsonIgnore]
    public long ApprenticeshipId { get; set; }
    public string ProviderName { get; set; }
    public BaseEdit OriginalApprenticeship { get; set; }
    public BaseEdit ApprenticeshipUpdates { get; set; }
}