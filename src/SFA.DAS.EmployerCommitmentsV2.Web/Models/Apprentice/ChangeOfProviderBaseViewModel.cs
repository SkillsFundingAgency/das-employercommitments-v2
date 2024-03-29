﻿using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class ChangeOfProviderBaseViewModel : IAuthorizationContextModel
{
    public string AccountHashedId { get; set; }
    public string ApprenticeshipHashedId { get; set; }
    public long ApprenticeshipId { get; set; }
    public long? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public bool? EmployerWillAdd { get; set; }
    public virtual int? NewStartMonth { get; set; }
    public virtual int? NewStartYear { get; set; }
    public virtual int? NewEndMonth { get; set; }
    public virtual int? NewEndYear { get; set; }
    public int? NewPrice { get; set; }
    public bool Edit { get; set; }
    public bool? StoppedDuringCoP { get; set; }
}